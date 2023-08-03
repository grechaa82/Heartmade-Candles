import React, { FC } from 'react';
import Tag, { TagData } from '../shared/Tag';

import Style from './InputTag.module.css';

interface InputTagProps {
  tags: TagData[];
  allTags: TagData[];
  onChange: (tags: TagData[]) => void;
  onDelete?: (id: string) => void;
}

const InputTag: FC<InputTagProps> = ({ tags, allTags, onChange, onDelete }) => {
  const [inputValue, setInputValue] = React.useState('');

  const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setInputValue(event.target.value);
  };

  const handleKeyDown = (event: React.KeyboardEvent<HTMLInputElement>) => {
    if (event.key === 'Enter') {
      if (allTags) {
        const tag = allTags.find((tag) => tag.text === inputValue);

        if (tag && !tags.includes(tag)) {
          onChange([...tags, tag]);
        }
      }
      setInputValue('');
    }
  };

  const handleRemoveTag = (tag: TagData) => {
    if (onDelete) {
      onDelete(tag.id.toString());
    }
    const newSelectedTags = tags.filter((t) => t !== tag);
    onChange(newSelectedTags);
  };

  return (
    <div className={Style.inputTag}>
      {tags.map((tag) => (
        <Tag
          key={tag.id}
          tag={tag}
          onRemove={() => handleRemoveTag(tag)}
          appearanceTag="secondary"
        />
      ))}
      <div className={Style.inputContainer}>
        <input type="text" value={inputValue} onChange={handleChange} onKeyDown={handleKeyDown} />
      </div>
    </div>
  );
};

export default InputTag;
