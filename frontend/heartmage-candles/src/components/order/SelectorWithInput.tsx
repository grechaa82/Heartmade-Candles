import { FC, ReactElement } from 'react';

import { IconProps } from '../../UI/IconProps';
import { InputProps } from '../shared/Input';

import Style from './SelectorWithInput.module.css';

export interface SelectorWithInputProps {
  title: string;
  icon?: FC<IconProps>;
  input: ReactElement<InputProps>;
  isSelected?: boolean;
  onSelected: () => void;
}

const SelectorWithInput: FC<SelectorWithInputProps> = ({
  title,
  icon: Icon,
  input,
  isSelected = false,
  onSelected,
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
      {isSelected && <div className={Style.inputBlock}>{input}</div>}
    </div>
  );
};

export default SelectorWithInput;
