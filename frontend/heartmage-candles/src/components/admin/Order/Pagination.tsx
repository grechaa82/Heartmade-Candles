import { FC } from 'react';

import IconChevronLeftLarge from '../../../UI/IconChevronLeftLarge';
import IconChevronRightLarge from '../../../UI/IconChevronRightLarge';

import Style from './Pagination.module.css';

export interface PaginationProps {
  totalCount: number;
  pageSize: number;
  currentPage: number;
  onPageChange: (page: number) => void;
}

const Pagination: FC<PaginationProps> = ({
  totalCount,
  pageSize,
  currentPage,
  onPageChange,
}) => {
  const totalPages = Math.ceil(totalCount / pageSize);
  const visiblePages = 7;
  const pageRange = 2;
  const startPage = Math.max(1, currentPage + 1 - pageRange);
  const endPage = Math.min(totalPages, currentPage + 1 + pageRange);

  const handlePageChange = (page: number) => {
    if (page >= 0 && page < totalPages) {
      onPageChange(page);
    }
  };

  const renderPagination = () => {
    const pages = [];

    if (totalPages <= visiblePages) {
      for (let i = 1; i <= totalPages; i++) {
        pages.push(
          <button
            key={i}
            onClick={() => handlePageChange(i - 1)}
            disabled={currentPage === i - 1}
            className={`${Style.pagginationBtn} ${
              currentPage === i - 1 ? Style.currentPage : ''
            }`}
          >
            {i}
          </button>
        );
      }
    } else {
      if (startPage > 1) {
        pages.push(
          <button
            key="first"
            onClick={() => handlePageChange(0)}
            className={Style.pagginationBtn}
          >
            1
          </button>
        );
        if (startPage > 2)
          pages.push(
            <span className={Style.pagginationBtn} key={`ellipsis-start`}>
              ...
            </span>
          );
      }

      for (let i = startPage; i <= endPage; i++) {
        pages.push(
          <button
            key={i}
            onClick={() => handlePageChange(i - 1)}
            disabled={currentPage === i - 1}
            className={`${Style.pagginationBtn} ${
              currentPage === i - 1 ? Style.currentPage : ''
            }`}
          >
            {i}
          </button>
        );
      }

      if (endPage < totalPages) {
        if (endPage < totalPages - 1)
          pages.push(
            <span className={Style.pagginationBtn} key={`ellipsis-end`}>
              ...
            </span>
          );
        pages.push(
          <button
            key="last"
            onClick={() => handlePageChange(totalPages - 1)}
            className={Style.pagginationBtn}
          >
            {totalPages}
          </button>
        );
      }
    }

    return pages;
  };

  return (
    <div className={Style.pagginationBlock}>
      <p className={Style.info}>
        Строка {currentPage * pageSize + 1} -{' '}
        {Math.min((currentPage + 1) * pageSize, totalCount)} из {totalCount}
      </p>
      <div className={Style.pagginationDiv}>
        <button
          key="prev"
          onClick={() => handlePageChange(currentPage - 1)}
          className={Style.pagginationIcon}
        >
          <IconChevronLeftLarge color="#aaa" />
        </button>
        {renderPagination()}
        <button
          key="next"
          onClick={() => handlePageChange(currentPage + 1)}
          className={Style.pagginationIcon}
        >
          <IconChevronRightLarge color="#aaa" />
        </button>
      </div>
    </div>
  );
};

export default Pagination;
