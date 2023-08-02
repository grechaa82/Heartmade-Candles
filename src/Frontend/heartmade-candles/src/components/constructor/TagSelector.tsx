import { FC } from 'react';

import Tag, { TagData } from '../shared/Tag';

import Style from './TagSelector.module.css';

interface TagSelectorProps {
  title: string;
  tags: TagData[];
  handleSelectTag?: (product: TagData) => void;
}

const TagSelector: FC<TagSelectorProps> = ({ title, tags, handleSelectTag }) => {
  return (
    <div className={Style.selectedTag}>
      <h2>{title}</h2>
      <div className={Style.tagGrid}>
        {tags.map((tag) => (
          <Tag key={tag.id} tag={tag} handleSelectTag={handleSelectTag} />
        ))}
      </div>
    </div>
  );
};

export default TagSelector;
