﻿using System;
using System.Collections.Generic;
using System.Web;
using ProductService.Models;
using System.Linq;
using System.Web.OData;

using System.Data.Entity;
using System.Data.Entity.Infrastructure;

using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace ProductService.Controllers
{
    public class SuppliersController : ODataController
    {
        ProductsContext db = new ProductsContext();


        //-----
        private bool SuppliersExists(int key)
        {
            return db.Suppliers.Any(p => p.Id == key);
        } 
        //---
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }


        // Other controller methods not shown.

        public async Task<IHttpActionResult> DeleteRef([FromODataUri] int key,
        [FromODataUri] string relatedKey, string navigationProperty)
        {
            var supplier = await db.Suppliers.SingleOrDefaultAsync(p => p.Id == key);
            if (supplier == null)
            {


                return StatusCode(HttpStatusCode.NotFound);

            }



            switch (navigationProperty)
            {
                case "Products":
                    var productId = Convert.ToInt32(relatedKey);
                    var product = await db.Products.SingleOrDefaultAsync(p => p.Id == productId);

                    if (product == null)
                    {
                        return NotFound();
                    }
                    product.Supplier = null;
                    break;
                default:
                    return StatusCode(HttpStatusCode.NotImplemented);

            }
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

   


        [EnableQuery]
        public SingleResult<Supplier> GetSupplier([FromODataUri] int key)
        {
            var result = db.Products.Where(m => m.Id == key).Select(m => m.Supplier);
            return SingleResult.Create(result);
        }


        // Other controller methods not shown.



        [EnableQuery]
        public IQueryable<Product> GetProducts([FromODataUri] int key)
        {
            return db.Suppliers.Where(m => m.Id.Equals(key)).SelectMany(m => m.Products);
        }

        


        [EnableQuery] 
        public IQueryable<Supplier> Get()
        {
            return db.Suppliers;
        }

        [EnableQuery]
        public SingleResult<Supplier> Get([FromODataUri] int key)
        {
            IQueryable<Supplier> result = db.Suppliers.Where(p => p.Id == key);
            return SingleResult.Create(result);
        }


        // Other controller methods not shown.

        public async Task<IHttpActionResult> Post(Supplier Supplier)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Suppliers.Add(Supplier);
            await db.SaveChangesAsync();
            return Created(Supplier);
        }




        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Supplier> Supplier)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = await db.Suppliers.FindAsync(key);
            if (entity == null)
            {
                return NotFound();
            }
            Supplier.Patch(entity);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SuppliersExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Updated(entity);
        }

       
        //56577 LOCALHOSTTT


        public async Task<IHttpActionResult> Put([FromODataUri] int key, Supplier update)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (key != update.Id)
            {
                return BadRequest();
            }
            db.Entry(update).State = EntityState.Modified;
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SuppliersExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Updated(update);
        }

       



        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            var Supplier = await db.Suppliers.FindAsync(key);
            if (Supplier == null)
            {
                return NotFound();
            }
            db.Suppliers.Remove(Supplier);
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}


