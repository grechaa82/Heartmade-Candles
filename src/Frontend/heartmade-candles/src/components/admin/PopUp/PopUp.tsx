import { FC, ReactNode } from "react";

import IconRemoveLarge from "../../../UI/IconRemoveLarge";

import Style from "./PopUp.module.css";

export interface PopUpProps {
  onClose: () => void;
  children?: ReactNode;
}

const PopUp: FC<PopUpProps> = ({ onClose, children }) => {
  return (
    <div className={Style.overlay}>
      <div className={Style.popUp}>
        <button className={Style.closeButton} onClick={onClose}>
          <IconRemoveLarge color="#777" />
        </button>
        {children}
      </div>
    </div>
  );
};

export default PopUp;
