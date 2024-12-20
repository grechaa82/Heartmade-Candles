import {
  FC,
  useState,
  useEffect,
  ReactNode,
  cloneElement,
  ReactElement,
} from 'react';

import InputTag from '../../components/admin/InputTag';
import { TagData } from '../../components/shared/Tag';
import Button from '../../components/shared/Button';

import Style from './TagsGrid.module.css';

export interface TagsGridProps {
  title: string;
  tags: TagData[];
  allTags?: TagData[];
  popUpComponent?: ReactNode;
  withInput?: boolean;
  onSave?: (saveProduct: TagData[]) => void;
  onDelete?: (id: string) => void;
}

const TagsGrid: FC<TagsGridProps> = ({
  title,
  tags,
  allTags,
  popUpComponent,
  withInput = true,
  onSave,
  onDelete,
}) => {
  const [selectedTags, setSelectedTags] = useState<TagData[]>([]);
  const [isModified, setIsModified] = useState(false);
  const [isPopUpOpen, setIsPopUpOpen] = useState(false);
  const [isOpen, setIsOpen] = useState(true);

  const toggleOpen = () => {
    setIsOpen((prev) => !prev);
  };

  const handlePopUpOpen = () => {
    setIsPopUpOpen(true);
  };

  const handlePopUpClose = () => {
    setIsPopUpOpen(false);
  };

  const handleChangesTags = (tags: TagData[]) => {
    setSelectedTags(tags);
    setIsModified(true);
  };

  const handleOnSave = () => {
    if (onSave) {
      onSave(selectedTags);
      setIsModified(false);
    }
  };

  useEffect(() => {
    setSelectedTags(tags);
  }, [tags]);

  return (
    <>
      <div className={Style.tabsInput}>
        <div className={Style.titleBlock}>
          <div className={Style.sectionHeader}>
            <h2 onClick={toggleOpen}>{title}</h2>
            {popUpComponent && (
              <Button
                text="Добавить"
                onClick={handlePopUpOpen}
                color="#2E67EA"
              />
            )}
          </div>
          <button className={Style.toggleButton} onClick={toggleOpen}>
            {isOpen ? '–' : '+'}
          </button>
        </div>
        {isOpen && (
          <div className={Style.content}>
            <InputTag
              tags={selectedTags}
              allTags={allTags}
              onChange={handleChangesTags}
              onDelete={onDelete}
              withInput={withInput}
            />
            {onSave && isModified && !popUpComponent && (
              <button
                type="button"
                className={Style.saveButton}
                onClick={handleOnSave}
              >
                Сохранить
              </button>
            )}
          </div>
        )}
      </div>
      {isPopUpOpen &&
        cloneElement(popUpComponent as ReactElement, {
          onClose: handlePopUpClose,
        })}
    </>
  );
};

export default TagsGrid;
