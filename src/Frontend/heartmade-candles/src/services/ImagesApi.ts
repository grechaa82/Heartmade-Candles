const apiUrl = 'http://localhost:5000/api/admin/images';

export const ImagesApi = {
  async uploadImages(files: File[]): Promise<string[]> {
    console.log('ImagesApi files', files);
    const formData = new FormData();
    for (let i = 0; i < files.length; i++) {
      formData.append('formImages', files[i]);
    }
    console.log('ImagesApi formData', formData);
    const response = await fetch(`${apiUrl}`, {
      method: 'POST',
      body: formData,
    });

    return (await response.json()) as string[];
  },
  async deleteImages(fileNames: string[]): Promise<void> {
    await fetch(`${apiUrl}`, {
      method: 'DELETE',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(fileNames),
    });
  },
};
