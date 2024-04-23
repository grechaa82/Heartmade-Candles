import { FC, ReactNode } from 'react';

import { IconProps } from '../../UI/IconProps';

import Style from './SelectorWithInput.module.css';

export interface SelectorWithInputProps {
  title: string;
  icon?: FC<IconProps>;
  isSelected?: boolean;
  onSelected: () => void;
  children?: ReactNode;
}

const SelectorWithInputV2: FC<SelectorWithInputProps> = ({
  title,
  icon: Icon,
  isSelected = false,
  onSelected,
  children,
}) => {
  return (
    <div className={Style.selectorWithInput} onClick={() => onSelected()}>
      <div className={Style.title}>
        {Icon && <Icon />}
        {title}
      </div>
      <div className={Style.selectedBlock}>
        {isSelected && <div className={Style.selected}></div>}
      </div>
      {isSelected && <div className={Style.inputBlock}>{children}</div>}
    </div>
  );
};

export default SelectorWithInputV2;
