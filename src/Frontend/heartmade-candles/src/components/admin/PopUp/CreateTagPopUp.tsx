import { FC, useState, ChangeEvent } from "react";

import Textarea from "../Textarea";
import PopUp, { PopUpProps } from "./PopUp";
import { TagData } from "../Tag";

import Style from "./CreateTagPopUp.module.css";

export interface CreateTagPopUpProps extends PopUpProps {
  title: string;
  isNumber?: boolean;
  onSave: (tag: TagData) => void;
}

const CreateTagPopUp: FC<CreateTagPopUpProps> = ({
  onClose,
  title,
  isNumber = false,
  onSave,
}) => {
  const [tag, setTag] = useState<TagData>({
    id: 0,
    text: "",
  });
  const [isModified, setIsModified] = useState(false);

  const handleChangeTitle = (event: ChangeEvent<HTMLTextAreaElement>) => {
    setTag((prev) => ({ ...prev, text: event.target.value }));
    setIsModified(true);
  };

  return (
    <PopUp onClose={onClose}>
      <div className={Style.cont}>
        <h2 className={Style.title}>{title}</h2>
        <form className={`${Style.gridContainer} ${Style.formForTag}`}>
          <div className={`${Style.formItem} ${Style.itemTitle}`}>
            <Textarea
              text={tag.text}
              label="Название"
              limitation={{ limit: 48 }}
              onChange={handleChangeTitle}
            />
          </div>
          {onSave && isModified && (
            <button
              type="button"
              className={Style.saveButton}
              onClick={() => {
                onSave(tag);
                onClose();
              }}
            >
              Сохранить
            </button>
          )}
        </form>
      </div>
    </PopUp>
  );
};

export default CreateTagPopUp;
