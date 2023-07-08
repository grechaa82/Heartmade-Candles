import { FC, useEffect, useState } from "react";
import { useParams } from "react-router-dom";

import MainInfoDecor from "../modules/MainInfoDecor";
import { Decor } from "../types/Decor";
import { DecorRequest } from "../types/Requests/DecorRequest";

import { DecorsApi } from "../services/DecorsApi";

type DecorParams = {
  id: string;
};

const DecorPage: FC = () => {
  const { id } = useParams<DecorParams>();
  const [decorData, setDecorData] = useState<Decor>();

  const handleChangesDecor = (updatedDecor: Decor) => {
    setDecorData((prevDecorData) => ({
      ...prevDecorData,
      ...updatedDecor,
    }));
  };

  const updateDecor = async (updatedItem: Decor) => {
    if (id) {
      const decorRequest: DecorRequest = {
        title: updatedItem.title,
        description: updatedItem.description,
        price: updatedItem.price,
        imageURL: updatedItem.imageURL,
        isActive: updatedItem.isActive,
      };
      await DecorsApi.update(id, decorRequest);
    }
  };

  useEffect(() => {
    async function fetchDecors() {
      if (id) {
        const data = await DecorsApi.getById(id);
        setDecorData(data);
      }
    }

    fetchDecors();
  }, [id]);

  return (
    <>
      <div className="decors">
        {decorData && (
          <MainInfoDecor
            data={decorData}
            handleChangesDecor={handleChangesDecor}
            onSave={updateDecor}
          />
        )}
      </div>
    </>
  );
};

export default DecorPage;
