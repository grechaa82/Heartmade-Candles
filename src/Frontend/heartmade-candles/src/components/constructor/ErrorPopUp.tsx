import { FC, useEffect } from 'react';

import IconRemoveLarge from '../../UI/IconRemoveLarge';

import Style from './ErrorPopUp.module.css';

export interface PopUpProps {
  message: string;
  onClose: () => void;
}

const ErrorPopUp: FC<PopUpProps> = ({ message, onClose }) => {
  useEffect(() => {
    const timer = setTimeout(() => {
      onClose();
    }, 5000);

    return () => {
      clearTimeout(timer);
    };
  }, [onClose]);

  return (
    <div className={Style.errorPopUp}>
      <span>{message}</span>
      <button className={Style.closeButton} onClick={onClose}>
        <IconRemoveLarge color="#EB5757" />
      </button>
    </div>
  );
};

export default ErrorPopUp;
