import React, { Fragment, useEffect } from "react";
import MetaData from "./layout/MetaData";
import { useDispatch, useSelector } from "react-redux";
import { getProducts } from "../actions/productsAction";
import Product from "./products/Product";
import Loader from "./layout/Loader";
import { useAlert } from "react-alert";

const Home = () => {

  const dispatch = useDispatch(); 

  //{products} INVOCA LA COLECCION DE PRODUCTOS & state.products INVOCA LA DATA QUE ESTA EN EL STORE
  const {products, loading, error} = useSelector((state) => state.products); 

  const alert = useAlert();


  useEffect (() => {
    
    if(error != null){
      
      return alert.error(error); 

    }


    dispatch(getProducts());

  }, [dispatch, alert, error])

  if(loading){
    return (<Loader/>); 
  }

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
