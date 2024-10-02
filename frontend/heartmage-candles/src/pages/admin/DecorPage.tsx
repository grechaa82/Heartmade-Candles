import { FC, useState } from 'react';
import { useParams } from 'react-router-dom';

import MainInfoDecor from '../../modules/admin/MainInfoDecor';
import ListErrorPopUp from '../../modules/shared/ListErrorPopUp';
import useDecorByIdQuery from '../../hooks/useDecorByIdQuery';

import Style from './DecorPage.module.css';

type DecorParams = {
  id: string;
};

const DecorPage: FC = () => {
  const { id } = useParams<DecorParams>();
  const { data, isLoading, updateDecor } = useDecorByIdQuery(id);
  const [errorMessage, setErrorMessage] = useState<string[]>([]);

  if (isLoading) {
    return <div>...Loading</div>;
  }

  return (
    <>
      <div className="decors">
        {data && <MainInfoDecor data={data} onSave={updateDecor} />}
      </div>
      <ListErrorPopUp messages={errorMessage} />
    </>
  );
};

export default DecorPage;
