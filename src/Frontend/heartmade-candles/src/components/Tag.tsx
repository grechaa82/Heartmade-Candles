import { FC } from "react";
import IconRemoveLarge from "../UI/IconRemoveLarge";

import Style from "./Tag.module.css";

export interface TagData {
  id: number;
  text: string;
}

export interface TagProps {
  tag: TagData;
  onRemove?: () => void;
}

const Tag: FC<TagProps> = ({ tag, onRemove }) => {
  const handleRemoveClick = () => {
    if (onRemove) {
      onRemove();
    }
  };

  return (
    <div className={Style.tag}>
      <p>{tag.text}</p>
      {onRemove && (
        <button onClick={handleRemoveClick}>
          <IconRemoveLarge color="#fff" />
        </button>
      )}
    </div>
  );
};

export default Tag;
