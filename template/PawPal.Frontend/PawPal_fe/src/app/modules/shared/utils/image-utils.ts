// src/app/shared/utils/image-utils.ts
export function base64ToBlobUrl(base64String: string): string {
  const base64Data = base64String.includes(',') ? base64String.split(',')[1] : base64String;
  const byteCharacters = atob(base64Data);
  const byteNumbers = Array.from(byteCharacters, (char) => char.charCodeAt(0));
  const byteArray = new Uint8Array(byteNumbers);

  // Quick MIME sniffing
  let mimeType = 'image/png';
  if (byteArray.length > 4) {
    const headerHex = Array.from(byteArray.slice(0, 4), (b) => b.toString(16).toUpperCase()).join(
      '',
    );
    if (headerHex.startsWith('89504E47')) mimeType = 'image/png';
    else if (headerHex.startsWith('FFD8FF')) mimeType = 'image/jpeg';
  }

  const blob = new Blob([byteArray], { type: mimeType });
  return URL.createObjectURL(blob);
}
