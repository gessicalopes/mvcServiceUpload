using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MvcServiceUpload.Models;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;


namespace MvcServiceUpload.Controllers
{
    public class UploadController : ApiController
    {

        private ServiceContext db = new ServiceContext();

        // GET api/upload
        public IEnumerable<Upload> GetUploads()
        {
            return db.Uploads.AsEnumerable();
        }

        // GET api/upload/5
        public Upload GetUpload(int id)
        {
             Upload upload = db.Uploads.Find(id);
             if (upload == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

             return upload;

        }

        // POST api/upload
        public HttpResponseMessage PostUpload(Upload upload)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    upload.ID = 1;
                    upload.Arquivo = null;
                    upload.Descricao = "teste";
                    db.Uploads.Add(upload);
                    db.SaveChanges();

                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, upload);
                    response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = upload.ID }));
                    return response;
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        // PUT api/upload/5
        public HttpResponseMessage PutUpload(int id, Upload upload)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != upload.ID)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(upload).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // DELETE api/pload/5
        public HttpResponseMessage DeleteUpload(int id)
        {
            Upload upload = db.Uploads.Find(id);
            if (upload == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Uploads.Remove(upload);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, upload);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}
