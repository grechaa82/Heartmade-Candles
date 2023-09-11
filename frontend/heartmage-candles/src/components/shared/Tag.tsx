import { FC } from 'react';
import IconRemoveLarge from '../../UI/IconRemoveLarge';

import Style from './Tag.module.css';

export interface TagData {
  id: number;
  text: string;
}

type AppearanceTag = 'grey' | 'outline' | 'secondary' | 'primary';

export interface TagProps {
  tag: TagData;
  onRemove?: () => void;
  onSelectTag?: (tag: TagData) => void;
  onDeselectTag?: (tag: TagData) => void;
  appearanceTag?: AppearanceTag;
  isSelected?: boolean;
}

const Tag: FC<TagProps> = ({
  tag,
  onRemove,
  onSelectTag,
  onDeselectTag,
  appearanceTag = 'outline',
  isSelected = false,
}) => {
  const handleSelectTag = () => {
    if (isSelected) {
      onDeselectTag && onDeselectTag(tag);
    } else {
      onSelectTag && onSelectTag(tag);
    }
  };

  return (
    <button
      className={`${Style.tag} ${Style[appearanceTag]} ${isSelected ? Style.selected : ''}`}
      onClick={() => handleSelectTag()}
      type="button"
    >
      <p className={Style.title}>{tag.text}</p>
      {onRemove && (
        <button onClick={onRemove} className={Style.removeIcon}>
          <IconRemoveLarge />
        </button>
      )}
    </button>
  );
};

export default Tag;
