import { FC, useEffect, useState } from 'react';
import { useInView } from 'react-intersection-observer';

import ProductsGrid from '../../modules/admin/ProductsGrid';
import CreateSmellPopUp from '../../modules/admin/PopUp/Smell/CreateSmellPopUp';
import ListErrorPopUp from '../../modules/shared/ListErrorPopUp';
import useSmellsQuery from '../../hooks/useSmellsQuery';

import Style from './AllSmellPage.module.css';

export interface AllSmellPageProps {}

const AllSmellPage: FC<AllSmellPageProps> = () => {
  const {
    data,
    isLoading,
    fetchNextPage,
    hasNextPage,
    isFetchingNextPage,
    createSmell,
    deleteSmell,
    updateIsActiveSmell,
  } = useSmellsQuery();
  const [errorMessage, setErrorMessage] = useState<string[]>([]);
  const { ref, inView, entry } = useInView({
    threshold: 0,
  });

  useEffect(() => {
    if (entry && inView) {
      fetchNextPage();
    }
  }, [entry]);

  if (isLoading) {
    return <div>...Loading</div>;
  }

  return (
    <>
      {isLoading}
      <ProductsGrid
        data={data?.pages.flat() || []}
        title="Запахи"
        pageUrl="smells"
        renderPopUpComponent={(onClose) => (
          <CreateSmellPopUp
            onClose={onClose}
            title="Создать запах"
            onSave={createSmell}
          />
        )}
        deleteProduct={deleteSmell}
        updateIsActiveProduct={updateIsActiveSmell}
      />
      {isFetchingNextPage ? (
        <span>...Loading</span>
      ) : (
        hasNextPage && <div ref={ref}></div>
      )}
      <ListErrorPopUp messages={errorMessage} />
    </>
  );
};

export default AllSmellPage;
