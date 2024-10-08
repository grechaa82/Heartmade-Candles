import { FC, ReactNode, useEffect, useState } from 'react';

import IconRemoveLarge from '../../../UI/IconRemoveLarge';

import Style from './PopUp.module.css';

export interface PopUpProps {
  onClose: () => void;
  children?: ReactNode;
}

const PopUp: FC<PopUpProps> = ({ onClose, children }) => {
  const [isOpen, setIsOpen] = useState(false);

  const handleOnClose = () => {
    setIsOpen(false);
    onClose();
  };

  useEffect(() => {
    if (isOpen) {
      document.body.classList.add('no-scroll');
    } else {
      document.body.classList.remove('no-scroll');
    }
    return () => {
      document.body.classList.remove('no-scroll');
    };
  }, [isOpen]);

  useEffect(() => {
    setIsOpen(true);
  }, []);

  return (
    <div className={Style.overlay}>
      <div className={Style.popUp}>
        <button className={Style.closeButton} onClick={handleOnClose}>
          <IconRemoveLarge color="#777" />
        </button>
        {children}
      </div>
    </div>
  );
};

export default PopUp;
