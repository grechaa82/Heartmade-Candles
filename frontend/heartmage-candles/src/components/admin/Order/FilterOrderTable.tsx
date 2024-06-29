import { FC, useState } from 'react';

import { OrderStatus } from '../../../typesV2/order/OrderStatus';
import ButtonDropdown from '../../shared/ButtonDropdown';
import Button from '../../shared/Button';
import IconRemoveLarge from '../../../UI/IconRemoveLarge';

import Style from './FilterOrderTable.module.css';

interface FilterOrderTableProps {
  onFilter: (filters: Partial<FilterParams>) => void;
}

interface FilterParams {
  createdFrom: Date | null;
  createdTo: Date | null;
  status: OrderStatus | null;
}

const FilterOrderTable: FC<FilterOrderTableProps> = ({ onFilter }) => {
  const [filters, setFilters] = useState<FilterParams>({
    createdFrom: null,
    createdTo: null,
    status: null,
  });

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFilters((prevFilters) => ({
      ...prevFilters,
      [name]:
        name === 'createdFrom' || name === 'createdTo'
          ? value
            ? new Date(value)
            : null
          : value || null,
    }));
  };

  const handleStatusChange = (
    selectedOption: { id: string; title: string } | null
  ) => {
    setFilters((prevFilters) => ({
      ...prevFilters,
      status: selectedOption
        ? (Number(selectedOption.id) as OrderStatus)
        : null,
    }));
  };

  const handleApplyFilters = () => {
    onFilter(filters);
  };

  const statusOptions: { id: string; title: string }[] = [
    { id: 'Все', title: 'Все' },
    { id: OrderStatus.Created.toString(), title: 'Создан' },
    { id: OrderStatus.Confirmed.toString(), title: 'Подтвержден' },
    { id: OrderStatus.Placed.toString(), title: 'Размещен' },
    { id: OrderStatus.Paid.toString(), title: 'Оплачен' },
    { id: OrderStatus.InProgress.toString(), title: 'В процессе' },
    { id: OrderStatus.Packed.toString(), title: 'Упакован' },
    { id: OrderStatus.InDelivery.toString(), title: 'Доставляется' },
    { id: OrderStatus.Completed.toString(), title: 'Завершен' },
    { id: OrderStatus.Cancelled.toString(), title: 'Отменен' },
  ];

  const selectedStatus =
    statusOptions.find(
      (option) => option.id === (filters.status?.toString() || 'Все')
    ) || statusOptions[0];

  const handleResetAllDate = () => {
    setFilters((prevFilters) => ({
      ...prevFilters,
      createdFrom: null,
      createdTo: null,
    }));
    handleApplyFilters();
  };

  const handleResetStatus = () => {
    setFilters((prevFilters) => ({
      ...prevFilters,
      status: null,
    }));
    handleApplyFilters();
  };

  return (
    <div className={Style.filterOrderTableBlock}>
      <div className={Style.dateBlock}>
        <input
          className={Style.dateInput}
          type="date"
          name="createdFrom"
          value={filters.createdFrom?.toISOString().slice(0, 10) || ''}
          onChange={handleInputChange}
        />
        <input
          className={Style.dateInput}
          type="date"
          name="createdTo"
          value={filters.createdTo?.toISOString().slice(0, 10) || ''}
          onChange={handleInputChange}
        />
        <button className={Style.resetBtn} onClick={handleResetAllDate}>
          <IconRemoveLarge color="#aaa" />
        </button>
      </div>
      <div className={Style.dateBlock}>
        <div className={Style.statusInput}>
          <ButtonDropdown
            text="Status"
            options={statusOptions}
            selected={selectedStatus}
            onChange={handleStatusChange}
          />
        </div>
        <button className={Style.resetBtn} onClick={handleResetStatus}>
          <IconRemoveLarge color="#aaa" />
        </button>
      </div>
      <Button
        text="Применить"
        className={Style.applyFiltersBtn}
        onClick={handleApplyFilters}
      />
    </div>
  );
};

export default FilterOrderTable;
