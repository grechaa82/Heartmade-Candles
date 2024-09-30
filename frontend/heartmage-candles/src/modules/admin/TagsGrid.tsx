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
import ButtonWithIcon from '../../components/shared/ButtonWithIcon';
import IconPlusLarge from '../../UI/IconPlusLarge';

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
    <div className={Style.tabsInput}>
      <div className={Style.titleBlock}>
        <h2>{title}</h2>
        {popUpComponent && (
          <ButtonWithIcon
            icon={IconPlusLarge}
            text="Добавить"
            onClick={handlePopUpOpen}
            color="#2E67EA"
          />
        )}
      </div>
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
        {isPopUpOpen &&
          cloneElement(popUpComponent as ReactElement, {
            onClose: handlePopUpClose,
          })}
      </div>
    </div>
  );
};

export default TagsGrid;
