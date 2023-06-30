import { FC, useState, ChangeEvent } from "react";

import { Smell } from "../types/Smell";
import Textarea from "../components/Textarea";
import CheckboxBlock from "../components/CheckboxBlock";

import Style from "./MainInfoSmell.module.css";

export interface MainInfoSmellProps {
  data: Smell;
  handleChangesSmell: (smell: Smell) => void;
  onSave?: (saveSmell: Smell) => void;
}

const MainInfoSmell: FC<MainInfoSmellProps> = ({
  data,
  handleChangesSmell,
  onSave,
}) => {
  const [smell, setSmell] = useState<Smell>(data);
  const [isModified, setIsModified] = useState(false);

  const handleChangeTitle = (event: ChangeEvent<HTMLTextAreaElement>) => {
    setSmell((prev) => ({ ...prev, title: event.target.value }));
    handleChangesSmell(smell);
    setIsModified(true);
  };

  const handleChangePrice = (event: ChangeEvent<HTMLTextAreaElement>) => {
    setSmell((prev) => ({
      ...prev,
      price: parseFloat(event.target.value),
    }));
    handleChangesSmell(smell);
    setIsModified(true);
  };

  const handleChangeIsActive = (isActive: boolean) => {
    setSmell((prev) => ({ ...prev, isActive }));
    handleChangesSmell(smell);
    setIsModified(true);
  };

  const handleChangeDescription = (event: ChangeEvent<HTMLTextAreaElement>) => {
    setSmell((prev) => ({ ...prev, description: event.target.value }));
    handleChangesSmell(smell);
    setIsModified(true);
  };

  return (
    <div className={Style.smellInfo}>
      <div className={Style.image}></div>
      <form className={`${Style.gridContainer} ${Style.formForSmell}`}>
        <div className={`${Style.formItem} ${Style.itemTitle}`}>
          <Textarea
            text={smell.title}
            label="Название"
            limitation={{ limit: 48 }}
            onChange={handleChangeTitle}
          />
        </div>
        <div className={`${Style.formItem} ${Style.itemPrice}`}>
          <Textarea
            text={smell.price.toString()}
            label="Стоимость"
            onChange={handleChangePrice}
          />
        </div>
        <div className={`${Style.formItem} ${Style.itemActive}`}>
          <CheckboxBlock
            text="Активна"
            checked={smell.isActive}
            onChange={handleChangeIsActive}
          />
        </div>
        <div className={`${Style.formItem} ${Style.itemDescription}`}>
          <Textarea
            text={smell.description}
            label="Описание"
            height={175}
            limitation={{ limit: 256 }}
            onChange={handleChangeDescription}
          />
        </div>
        {onSave && isModified && (
          <button
            type="button"
            className={Style.saveButton}
            onClick={() => {
              onSave(smell);
              setIsModified(false);
            }}
          >
            Сохранить
          </button>
        )}
      </form>
    </div>
  );
};

export default MainInfoSmell;
