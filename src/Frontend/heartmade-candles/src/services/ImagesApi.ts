const apiUrl = 'http://localhost:5000/api/admin/images';

export const ImagesApi = {
  async uploadImages(files: File[]): Promise<string[]> {
    const formData = new FormData();
    for (let i = 0; i < files.length; i++) {
      formData.append('formImages', files[i]);
    }
    const response = await fetch(`${apiUrl}`, {
      method: 'POST',
      headers: {
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
        Authorization: `Bearer ${localStorage.getItem('token')}`,
      },
      body: JSON.stringify(fileNames),
    });
  },
};
