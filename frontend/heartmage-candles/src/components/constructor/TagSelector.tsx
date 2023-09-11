import { FC } from 'react';

import Tag, { TagData } from '../shared/Tag';

import Style from './TagSelector.module.css';

interface TagSelectorProps {
  title: string;
  data: TagData[];
  selectedData?: TagData[];
  onSelectTag?: (product: TagData) => void;
  onDeselectTag?: (product: TagData) => void;
}

const TagSelector: FC<TagSelectorProps> = ({
  title,
  data,
  selectedData,
  onSelectTag,
  onDeselectTag,
}) => {
  return (
    <div className={Style.selectedTag}>
      <h2>{title}</h2>
      <div className={Style.tagGrid}>
        {data.map((tag) => (
          <Tag
            key={tag.id}
            tag={tag}
            onSelectTag={onSelectTag}
            onDeselectTag={onDeselectTag}
            isSelected={selectedData?.some((selectedData) => selectedData.id === tag.id)}
          />
        ))}
      </div>
    </div>
  );
};

export default TagSelector;
