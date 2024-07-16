import { FC } from 'react';

import ButtonDropdown, { optionData } from '../../shared/ButtonDropdown';

import Style from './RowsPerPageOptions.module.css';

export interface RowsPerPageOptionsProps {
  rows: number[];
  selected: number;
  onChange: (value: number) => void;
}

const RowsPerPageOptions: FC<RowsPerPageOptionsProps> = ({
  rows,
  selected,
  onChange,
}) => {
  const rowsOptions: optionData[] = rows.map((row, index) => ({
    id: index.toString(),
    title: row.toString(),
  }));

  const selectedRows: optionData = {
    id: selected.toString(),
    title: selected.toString(),
  };

  const handleStatusChange = (
    selectedOption: { id: string; title: string } | null,
  ) => {
    onChange(parseInt(selectedOption.title));
  };
  return (
    <ButtonDropdown
      text="Rows"
      options={rowsOptions}
      selected={selectedRows}
      onChange={handleStatusChange}
      size="m"
    />
  );
};

export default RowsPerPageOptions;
