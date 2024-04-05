import { ApiResponse } from './ApiResponse';
import { AuthHelper } from '../helpers/AuthHelper';

import { apiUrl } from '../config';

export const ImagesApi = {
  uploadImages: async (files: File[]): Promise<ApiResponse<string[]>> => {
    try {
      const formData = new FormData();
      for (let i = 0; i < files.length; i++) {
        formData.append('formImages', files[i]);
      }

      const authorizationString = AuthHelper.getAuthorizationString();
      const response = await fetch(`${apiUrl}/admin/images`, {
        method: 'POST',
        headers: {
          Authorization: authorizationString,
        },
        body: formData,
      });

      if (response.ok) {
        return { data: await response.json(), error: null };
      } else {
        return { data: null, error: await response.text() };
      }
    } catch (error) {
      throw new Error(error as string);
    }
  },
  deleteImages: async (fileNames: string[]): Promise<ApiResponse<void>> => {
    try {
      const authorizationString = AuthHelper.getAuthorizationString();
      const response = await fetch(`${apiUrl}/admin/images`, {
        method: 'DELETE',
        headers: {
          'Content-Type': 'application/json',
          Authorization: authorizationString,
        },
        body: JSON.stringify(fileNames),
      });

      if (response.ok) {
        return { data: null, error: null };
      } else {
        return { data: null, error: await response.text() };
      }
    } catch (error) {
      throw new Error(error as string);
    }
  },
};
