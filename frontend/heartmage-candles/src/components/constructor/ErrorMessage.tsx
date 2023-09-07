import { FC, useEffect, useState } from 'react';

import IconRemoveLarge from '../../UI/IconRemoveLarge';

import Style from './ErrorMessage.module.css';

export interface ErrorMessageProps {
  message: string;
}

const ErrorMessage: FC<ErrorMessageProps> = ({ message }) => {
  const [isVisible, setIsVisible] = useState(true);

  const closeMessage = () => {
    setIsVisible(false);
  };

  useEffect(() => {
    const timer = setTimeout(() => {
      setIsVisible(false);
    }, 5000);

    return () => clearTimeout(timer);
  }, []);

  return (
    <div className={`${Style.errorMessage} ${isVisible ? Style.visible : Style.nonVisible}`}>
      <span>{message}</span>
      <button className={Style.closeButton} onClick={closeMessage}>
        <IconRemoveLarge color="#EB5757" />
      </button>
    </div>
  );
};

export default ErrorMessage;
