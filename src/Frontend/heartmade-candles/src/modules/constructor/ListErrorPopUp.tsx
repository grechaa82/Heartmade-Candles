import { FC } from 'react';

import ErrorMessage from '../../components/constructor/ErrorMessage';

import Style from './ListErrorPopUp.module.css';

export interface ListErrorPopUpProps {
  messages: string[];
}

const ListErrorPopUp: FC<ListErrorPopUpProps> = ({ messages }) => {
  console.log(messages);
  return (
    <div className={Style.listErrorPopUp}>
      {messages.map((message, index) => (
        <ErrorMessage key={index} message={message} />
      ))}
    </div>
  );
};

export default ListErrorPopUp;
