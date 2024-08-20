import { FC, useState } from 'react';

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
  const [isOpen, setIsOpen] = useState(true);

  const toggleOpen = () => {
    setIsOpen((prev) => !prev);
  };

  return (
    <div className={Style.selectedTag}>
      <div className={Style.headerContainer}>
        <h2 className={Style.sectionHeader} onClick={toggleOpen}>
          {title}
        </h2>
        <button className={Style.toggleButton} onClick={toggleOpen}>
          {isOpen ? '–' : '+'}
        </button>
      </div>
      {isOpen && (
        <div className={Style.tagGrid}>
          {data.map((tag) => (
            <Tag
              key={tag.id}
              tag={tag}
              onSelectTag={onSelectTag}
              onDeselectTag={onDeselectTag}
              isSelected={selectedData?.some(
                (selectedData) => selectedData?.id === tag.id,
              )}
            />
          ))}
        </div>
      )}
    </div>
  );
};

export default TagSelector;
