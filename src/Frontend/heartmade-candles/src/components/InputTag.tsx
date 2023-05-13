import React, { FC } from 'react';
import Tag, { TagProps } from './Tag';

import Style from './InputTag.module.css';

interface InputTagProps {}

const InputTag: React.FC<InputTagProps> = () => {
  const [tags, setTags] = React.useState<TagProps[]>([]);
  const [inputValue, setInputValue] = React.useState('');

  //TODO: Implement adding by "Enter" and by the comma sign ","

  const handleAddTag = () => {
    if (inputValue) {
      const newTag: TagProps = {
        id: tags.length,
        text: inputValue,
        removable: true,
      };
      setTags([...tags, newTag]);
      setInputValue('');
    }
  };

  const handleRemoveTag = (id: number) => {
    setTags(tags.filter((tag) => tag.id !== id));
  };

  return (
    <div className={Style.inputTag}>
      {tags.map((tag) => (
        <Tag key={tag.id} {...tag} removeTag={() => handleRemoveTag(tag.id)} />
      ))}
      <div className={Style.inputContainer}>
        <input
          placeholder="Type here to add new tag"
          value={inputValue}
          onChange={(e) => setInputValue(e.target.value)}
        />
        <button onClick={handleAddTag}>add</button>
      </div>
    </div>
  );
};

export default InputTag;
