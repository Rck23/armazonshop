import React, { Fragment, useEffect } from "react";
import MetaData from "./layout/MetaData";
import { useDispatch, useSelector } from "react-redux";
import { getProducts } from "../actions/productsAction";
import Product from "./products/Product";

const Home = () => {

  const dispatch = useDispatch(); 

  useEffect (() => {
    dispatch(getProducts());
  }, [dispatch])

  //{products} INVOCA LA COLECCION DE PRODUCTOS & state.products INVOCA LA DATA QUE ESTA EN EL STORE
  const {products} = useSelector((state) => state.products); 

  return (
    <Fragment>
      <MetaData titulo={"Los mejores productos"}/>
      <section id="products" className="container mt-5">
        <div className="row">

          {products ? products.map((productElement) => (
            <Product key={productElement.id} product={productElement} col={4}/>
            
              )
            ) : 'No tiene elementos products'
          }
        </div>
      </section>
    </Fragment>
  );
};

export default Home;
