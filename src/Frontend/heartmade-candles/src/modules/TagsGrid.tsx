import { FC, useState, useEffect } from "react";
import InputTag from "../components/InputTag";
import { TagData } from "../components/Tag";

import Style from "./TagsGrid.module.css";

export interface TagsGridProps {
  title: string;
  tags: TagData[];
  allTags?: TagData[];
  onChanges?: (updatedTags: TagData[]) => void;
  onSave?: (saveProduct: TagData[]) => void;
}

const TagsGrid: FC<TagsGridProps> = ({
  title,
  tags,
  allTags,
  onChanges,
  onSave,
}) => {
  const [selectedTags, setSelectedTags] = useState<TagData[]>([]);
  const [isModified, setIsModified] = useState(false);

  const handleChangesTags = (tags: TagData[]) => {
    setSelectedTags(tags);
    if (onChanges) {
      onChanges(tags);
    }
    setIsModified(true);
  };

  const handleOnSave = () => {
    if (onSave) {
      onSave(selectedTags);
      if (onChanges) {
        onChanges(selectedTags);
      }
      setIsModified(false);
    }
  };

  useEffect(() => {
    setSelectedTags(tags);
  }, [tags]);

  return (
    <div className={Style.tabsInput}>
      <h2>{title}</h2>
      <InputTag
        tags={selectedTags}
        allTags={allTags || []}
        onChange={handleChangesTags}
      />
      {onSave && isModified && (
        <button
          type="button"
          className={Style.saveButton}
          onClick={handleOnSave}
        >
          Сохранить
        </button>
      )}
    </div>
  );
};

export default TagsGrid;
