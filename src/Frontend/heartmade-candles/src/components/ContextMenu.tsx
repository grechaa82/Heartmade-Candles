import { FC } from "react";

import Style from "./ContextMenu.module.css";

export type Action = {
  label: string;
  onClick: () => void;
};

export interface ContextMenuProps {
  actions: Action[];
}

const ContextMenu: FC<ContextMenuProps> = ({ actions }) => {
  return (
    <div className={Style.contextMenu}>
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
