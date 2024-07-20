import { FC } from 'react';

import Style from './ContextMenu.module.css';

export type Action = {
  label: string;
  onClick: () => void;
};

export interface ContextMenuProps {
  header?: string;
  className?: string;
  actions: Action[];
}

const ContextMenu: FC<ContextMenuProps> = ({ header, className, actions }) => {
  return (
    <div className={`${Style.contextMenu} ${className ? className : ''}`}>
      {header && <p className={Style.header}>{header}</p>}
      <ul>
        {actions.map((action, index) => (
          <li
            className={Style.contextMenuItem}
            key={index}
            onClick={action.onClick}
          >
            {action.label}
          </li>
        ))}
      </ul>
    </div>
  );
};

export default ContextMenu;
