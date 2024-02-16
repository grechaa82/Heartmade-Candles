import { ApiResponse } from './ApiResponse';

import { apiUrl } from '../config';

export const BotApi = {
  getChatIdsByRole: async (role: number): Promise<ApiResponse<number[]>> => {
    try {
      const response = await fetch(`${apiUrl}/bot/chat?role=${role}`, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${localStorage.getItem('token')}`,
        },
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

  upgradeChatRole: async (chatIds: number[]): Promise<ApiResponse<void>> => {
    try {
      const response = await fetch(`${apiUrl}/bot/chat/upgrade`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${localStorage.getItem('token')}`,
        },
        body: JSON.stringify(chatIds),
      });

      if (response.ok) {
        return { data: null, error: null };
      } else {
        return { data: null, error: await response.text() };
      }
    } catch (error) {
      console.log(error);
      throw new Error(error as string);
    }
  },
};
