#!/usr/bin/env bash
set -euo pipefail

# ===================== Default settings =====================

# 1) Path to Identity project (where .csproj is and where user-secrets will be stored)
PROJECT_DIR=${1:-"../Services/Identity/Presentation"}

# 2) Directory for certificates
CERTS_DIR=${2:-"$PROJECT_DIR/certs"}

# 3) Base name for certificates/keys
CERT_NAME=${3:-"jwt-dev"}

# 4) Certificate lifetime (days)
DAYS=${4:-365}

# 5) User-secrets key
KEY=${5:-"Jwt:SigningCertificatePassword"}

# ===================== Pre-checks =====================

if ! command -v openssl >/dev/null 2>&1; then
  echo "Error: 'openssl' not found. Install openssl and try again." >&2
  exit 1
fi

if ! command -v dotnet >/dev/null 2>&1; then
  echo "Error: 'dotnet' not found. Install .NET SDK and try again." >&2
  exit 1
fi

if [ ! -d "$PROJECT_DIR" ]; then
  echo "Error: project directory '$PROJECT_DIR' does not exist." >&2
  exit 1
fi

mkdir -p "$CERTS_DIR"

KEY_FILE="$CERTS_DIR/$CERT_NAME.key"
CRT_FILE="$CERTS_DIR/$CERT_NAME.crt"
PFX_FILE="$CERTS_DIR/$CERT_NAME.pfx"
CER_FILE="$CERTS_DIR/$CERT_NAME.cer"

# ===================== Password input =====================

echo "=== Generating JWT certificate for project: $PROJECT_DIR ==="
echo "Certificates will be stored in: $CERTS_DIR"
echo "Certificate common name (CN): $CERT_NAME"
echo

read -s -p "Enter PFX password: " PFX_PASSWORD
echo
read -s -p "Confirm PFX password: " PFX_PASSWORD_CONFIRM
echo

if [[ "$PFX_PASSWORD" != "$PFX_PASSWORD_CONFIRM" ]]; then
  echo "Passwords do not match." >&2
  exit 1
fi

# ===================== Key and certificate generation =====================

echo "Generating private key..."
openssl genrsa -out "$KEY_FILE" 4096

echo "Generating self-signed certificate..."
openssl req -new -x509 \
  -key "$KEY_FILE" \
  -out "$CRT_FILE" \
  -days "$DAYS" \
  -subj "/CN=$CERT_NAME"

# ===================== Create PFX =====================

echo "Creating PKCS#12 (.pfx)..."
# Without -passout, openssl would ask for the password interactively,
# but we want to reuse the password that was already entered.
# To avoid showing the password in the process arguments, we pass it via fd:3.

# Open a temporary file descriptor with the password
exec 3<<<"$PFX_PASSWORD"

openssl pkcs12 -export \
  -out "$PFX_FILE" \
  -inkey "$KEY_FILE" \
  -in "$CRT_FILE" \
  -name "$CERT_NAME" \
  -passout fd:3

# Close the file descriptor
exec 3<&-

# ===================== Create CER (public certificate) =====================

echo "Creating public certificate (.cer)..."
openssl x509 -in "$CRT_FILE" -outform der -out "$CER_FILE"

# ===================== Cleanup temporary files =====================

rm -f "$KEY_FILE" "$CRT_FILE"

# ===================== Configure user-secrets =====================

echo "Configuring dotnet user-secrets..."

pushd "$PROJECT_DIR" >/dev/null

# Initialize user-secrets (ignore error if already initialized)
dotnet user-secrets init >/dev/null 2>&1 || true

dotnet user-secrets set "$KEY" "$PFX_PASSWORD" >/dev/null

popd >/dev/null

# ===================== Result =====================

echo
echo "Done."
echo "  Private PFX: $PFX_FILE"
echo "  Public CER:  $CER_FILE"
echo
echo "Password has been stored in user-secrets of project: $PROJECT_DIR (key: $KEY)"
echo
echo "Remember to set PFX path in appsettings.Development.json, for example:"
echo
echo "  \"Jwt\": {"
echo "    \"SigningCertificatePath\": \"certs/$CERT_NAME.pfx\""
echo "  }"
echo
echo "And in other services set CER path, for example:"
echo
echo "  \"Jwt\": {"
echo "    \"SigningCertificatePath\": \"certs/$CERT_NAME.cer\""
echo "  }"