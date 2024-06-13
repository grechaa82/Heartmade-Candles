import { FC, useEffect, useState } from 'react';

import TagsGrid from '../../modules/admin/TagsGrid';
import { TagData } from '../../components/shared/Tag';
import CreateTagPopUp from '../../components/admin/PopUp/Tag/CreateTagPopUp';

import { BotApi } from '../../services/BotApi';

import Style from './BotPage.module.css';

export interface BotPageProps {}

const BotPage: FC<BotPageProps> = () => {
  const [chatIds, setChatIds] = useState<number[]>([]);
  const [errorMessage, setErrorMessage] = useState<string[]>([]);

  const handleCreateChatId = async (tag: TagData): Promise<void> => {
    const newId = parseInt(tag.text);
    if (!chatIds.includes(newId)) {
      const updatedChatIds = [...chatIds, newId];
      const response = await BotApi.upgradeChatRole(updatedChatIds);
      if (response.error) {
        setErrorMessage([...errorMessage, response.error as string]);
      }
      const candlesResponse = await BotApi.getChatIdsByRole(1);
      if (candlesResponse.data && !candlesResponse.error) {
        setChatIds(candlesResponse.data);
      } else {
        setErrorMessage([...errorMessage, candlesResponse.error as string]);
      }
    } else {
      setErrorMessage([...errorMessage, `Id: ${newId} уже существует`]);
    }
  };

  const handleDeleteChatId = async (id: string) => {
    const idToRemove = parseInt(id);
    const updatedChatIds = chatIds.filter((chatId) => chatId !== idToRemove);

    const response = await BotApi.upgradeChatRole(updatedChatIds);
    if (response.error) {
      setErrorMessage([...errorMessage, response.error as string]);
    }
    const candlesResponse = await BotApi.getChatIdsByRole(1);
    if (candlesResponse.data && !candlesResponse.error) {
      setChatIds(candlesResponse.data);
    } else {
      setErrorMessage([...errorMessage, candlesResponse.error as string]);
    }
  };

  useEffect(() => {
    async function fetchData() {
      const candlesResponse = await BotApi.getChatIdsByRole(1);
      if (candlesResponse.data && !candlesResponse.error) {
        setChatIds(candlesResponse.data);
      } else {
        setErrorMessage([...errorMessage, candlesResponse.error as string]);
      }
    }

    fetchData();
  }, []);

  return (
    <>
      <TagsGrid
        title="Админ чаты"
        tags={convertChatIdsToTagData(chatIds)}
        onDelete={handleDeleteChatId}
        withInput={false}
        popUpComponent={
          <CreateTagPopUp
            onClose={() => console.log('Popup closed')}
            title="Добавить чат"
            onSave={handleCreateChatId}
          />
        }
      />
      <div></div>
    </>
  );
};

export default BotPage;

export function convertChatIdsToTagData(chatIds: number[]): TagData[] {
  return chatIds.map((chatId) => ({
    id: chatId,
    text: chatId.toString(),
  }));
}
