const apiUrl = 'http://localhost:80/api/admin/images';

export const ImagesApi = {
  async uploadImages(files: File[]): Promise<string[]> {
    const formData = new FormData();
    for (let i = 0; i < files.length; i++) {
      formData.append('formImages', files[i]);
    }
    const response = await fetch(`${apiUrl}`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
      body: formData,
    });

    return (await response.json()) as string[];
  },
  async deleteImages(fileNames: string[]): Promise<void> {
    await fetch(`${apiUrl}`, {
      method: 'DELETE',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
      body: JSON.stringify(fileNames),
    });
  },
};
