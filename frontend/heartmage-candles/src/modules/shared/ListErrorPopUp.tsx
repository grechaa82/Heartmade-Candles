import { FC } from 'react';

import ErrorMessage from '../../components/constructor/ErrorMessage';

import Style from './ListErrorPopUp.module.css';

export interface ListErrorPopUpProps {
  messages: string[];
}

const ListErrorPopUp: FC<ListErrorPopUpProps> = ({ messages }) => {
  return (
    <div className={Style.popUpNotification}>
      <div className={Style.listErrorPopUp}>
        {messages.map((message, index) => (
          <ErrorMessage key={index} message={message} />
        ))}
      </div>
    </div>
  );
};

export default ListErrorPopUp;
