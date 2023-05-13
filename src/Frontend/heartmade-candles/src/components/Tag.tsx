import React, { FC, useState } from 'react';
import IconRemoveLarge from '../UI/IconRemoveLarge';

import Style from './Tag.module.css';

export interface TagProps {
  id: number;
  text: string;
  removable?: boolean;
  removeTag?: () => void;
}

const Tag: React.FC<TagProps> = ({ id, text, removable = true, removeTag }) => {
  const [isRemoved, setIsRemoved] = React.useState(false);

  const handleRemoveClick = () => {
    setIsRemoved(true);
    if (removeTag) {
      removeTag();
    }
  };

  if (isRemoved) {
    return null;
  }

  return (
    <div className={Style.tag}>
      <p>{text}</p>
      {removable && (
        <button onClick={handleRemoveClick}>
          <IconRemoveLarge color="#fff" />
        </button>
      )}
    </div>
  );
};

export default Tag;
