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
  handleSelectTag?: (tag: TagData) => void;
  appearanceTag?: AppearanceTag;
}

const Tag: FC<TagProps> = ({ tag, onRemove, handleSelectTag, appearanceTag = 'outline' }) => {
  const handleClick = () => {
    if (handleSelectTag) {
      handleSelectTag(tag);
    }
  };

  const tagClassName = `${Style.tag} ${Style[appearanceTag]}`;

  return (
    <div className={tagClassName} onClick={handleClick}>
      <p className={Style.title}>{tag.text}</p>
      {onRemove && (
        <button onClick={onRemove} className={Style.removeIcon}>
          <IconRemoveLarge />
        </button>
      )}
    </div>
  );
};

export default Tag;
