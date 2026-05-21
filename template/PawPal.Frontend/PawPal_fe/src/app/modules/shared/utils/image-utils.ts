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

/*
  loadBlob(items: GetImagePostBlob) {
    this.newImage = false;
    items.postImages.forEach((base64String: string) => {
      const byteCharacters = atob(base64String);
      const byteNumbers = new Array(byteCharacters.length);
      for (let i = 0; i < byteCharacters.length; i++) {
        byteNumbers[i] = byteCharacters.charCodeAt(i);
      }
      const byteArray = new Uint8Array(byteNumbers);
      let mimeType = 'image/png';
      if (byteArray.length > 4) {
        const header = byteArray.slice(0, 4);
        let headerHex = '';
        for (let i = 0; i < header.length; i++) {
          headerHex += header[i].toString(16).toUpperCase();
        }
        if (headerHex.startsWith('89504E47')) {
          mimeType = 'image/png';
        } else if (headerHex.startsWith('FFD8FF')) {
          mimeType = 'image/jpeg';
        }
      }
      const blob = new Blob([byteArray], { type: mimeType });
      const imageUrl = URL.createObjectURL(blob);
      this.imageControls.push(this._formBuilder.control(imageUrl));
    });
    this.newImage = true;
    this.createFiles(items);
  }
    */
