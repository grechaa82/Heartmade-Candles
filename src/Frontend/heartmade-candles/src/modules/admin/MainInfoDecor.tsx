import { FC, useState, ChangeEvent } from "react";

import { Decor } from "../../types/Decor";
import Textarea from "../../components/admin/Textarea";
import CheckboxBlock from "../../components/admin/CheckboxBlock";

import Style from "./MainInfoDecor.module.css";

export interface MainInfoDecorProps {
  data: Decor;
  handleChangesDecor: (decor: Decor) => void;
  onSave?: (saveDecor: Decor) => void;
}

const MainInfoDecor: FC<MainInfoDecorProps> = ({
  data,
  handleChangesDecor,
  onSave,
}) => {
  const [decor, setDecor] = useState<Decor>(data);
  const [isModified, setIsModified] = useState(false);

  const handleChangeTitle = (event: ChangeEvent<HTMLTextAreaElement>) => {
    setDecor((prev) => ({ ...prev, title: event.target.value }));
    handleChangesDecor(decor);
    setIsModified(true);
  };

  const handleChangePrice = (event: ChangeEvent<HTMLTextAreaElement>) => {
    setDecor((prev) => ({
      ...prev,
      price: parseFloat(event.target.value),
    }));
    handleChangesDecor(decor);
    setIsModified(true);
  };

  const handleChangeIsActive = (isActive: boolean) => {
    setDecor((prev) => ({ ...prev, isActive }));
    handleChangesDecor(decor);
    setIsModified(true);
  };

  const handleChangeDescription = (event: ChangeEvent<HTMLTextAreaElement>) => {
    setDecor((prev) => ({ ...prev, description: event.target.value }));
    handleChangesDecor(decor);
    setIsModified(true);
  };

  return (
    <div className={Style.decorInfo}>
      <div className={Style.image}>
        <img src={decor.imageURL} />
      </div>
      <form className={`${Style.gridContainer} ${Style.formForDecor}`}>
        <div className={`${Style.formItem} ${Style.itemTitle}`}>
          <Textarea
            text={decor.title}
            label="Название"
            limitation={{ limit: 48 }}
            onChange={handleChangeTitle}
          />
        </div>
        <div className={`${Style.formItem} ${Style.itemPrice}`}>
          <Textarea
            text={decor.price.toString()}
            label="Стоимость"
            onChange={handleChangePrice}
          />
        </div>
        <div className={`${Style.formItem} ${Style.itemActive}`}>
          <CheckboxBlock
            text="Активна"
            checked={decor.isActive}
            onChange={handleChangeIsActive}
          />
        </div>
        <div className={`${Style.formItem} ${Style.itemDescription}`}>
          <Textarea
            text={decor.description}
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
              onSave(decor);
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

export default MainInfoDecor;
