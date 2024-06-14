import { FC } from 'react';

import PopUp, { PopUpProps } from '../../../../components/admin/PopUp/PopUp';
import { TagData } from '../../../../components/shared/Tag';
import TagForm from '../../../../components/admin/Form/Tag/TagForm';

import Style from './CreateTagPopUp.module.css';

export interface CreateTagPopUpProps extends PopUpProps {
  title: string;
  onSave: (tag: TagData) => void;
}

const CreateTagPopUp: FC<CreateTagPopUpProps> = ({
  onClose,
  title,
  onSave,
}) => {
  const handleOnSubmit = (data: TagData) => {
    const newTagData: TagData = {
      id: data.id,
      text: data.text,
    };
    onSave(newTagData);
    onClose();
  };

  return (
    <PopUp onClose={onClose}>
      <div className={Style.container}>
        <p className={Style.title}>{title}</p>
        <TagForm onSubmit={handleOnSubmit} />
      </div>
    </PopUp>
  );
};

export default CreateTagPopUp;
