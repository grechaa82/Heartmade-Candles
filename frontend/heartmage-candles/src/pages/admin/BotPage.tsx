import { FC, useEffect, useState } from 'react';

import TagsGrid from '../../modules/admin/TagsGrid';
import { TagData } from '../../components/shared/Tag';

import { BotApi } from '../../services/BotApi';

import Style from './BotPage.module.css';

export interface BotPageProps {}

const BotPage: FC<BotPageProps> = () => {
  const [chatIds, setChatIds] = useState<number[]>([]);
  const [errorMessage, setErrorMessage] = useState<string[]>([]);

  const updateChatIds = async (updatedItems: TagData[]) => {
    const response = await BotApi.upgradeChatRole(chatIds);

    if (response.error) {
      setErrorMessage([...errorMessage, response.error as string]);
    }
  };
  const handleChangesChatIds = (updatedItems: TagData[]): void => {
    const newIds = updatedItems.map((tagData) => parseInt(tagData.text));

    const updatedChatIds = chatIds.filter((id) => newIds.includes(id));

    setChatIds(updatedChatIds);
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
        title="Чаты с правами администратора"
        tags={convertChatIdsToTagData(chatIds)}
        onSave={updateChatIds}
        onChanges={handleChangesChatIds}
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
