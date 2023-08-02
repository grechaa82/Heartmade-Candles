import { FC } from 'react';
import IconRemoveLarge from '../../UI/IconRemoveLarge';

import Style from './Tag.module.css';

export interface TagData {
  id: number;
  text: string;
}

export interface TagProps {
  tag: TagData;
  onRemove?: () => void;
  handleSelectTag?: (tag: TagData) => void;
}

const Tag: FC<TagProps> = ({ tag, onRemove, handleSelectTag }) => {
  const handleRemoveClick = () => {
    if (onRemove) {
      onRemove();
    }
  };

  if (handleSelectTag) {
    return (
      <button className={Style.tag} onClick={() => handleSelectTag(tag)}>
        <p>{tag.text}</p>
      </button>
    );
  }
  return (
    <div className={Style.tag}>
      <p>{tag.text}</p>
      {onRemove && (
        <button onClick={handleRemoveClick}>
          <IconRemoveLarge color="#fff" />
        </button>
      )}
    </div>
  );
};

export default Tag;
