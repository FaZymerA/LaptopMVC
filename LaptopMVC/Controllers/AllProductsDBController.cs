using LaptopMVC.Models;
using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LaptopMVC.Controllers
{
    public class AllProductsDBController : Controller
    {
        Entities DB = new Entities();
        // GET: AllProductsDB

        public ActionResult Index(string searchStringUserNameOrEmail, string currentFilter, int? page)
        {


            List<Product> Lproducts = DB.Product.ToList();
            try
            {
                int intPage = 1;
                int intPageSize = 10;
                int intTotalPageCount = 0;

                if (searchStringUserNameOrEmail != null)
                {
                    intPage = 1;
                }
                else
                {
                    if (currentFilter != null)
                    {
                        searchStringUserNameOrEmail = currentFilter;
                        intPage = page ?? 1;
                    }
                    else
                    {
                        searchStringUserNameOrEmail = "";
                        intPage = page ?? 1;
                    }
                }

                ViewBag.CurrentFilter = searchStringUserNameOrEmail;



                int intSkip = (intPage - 1) * intPageSize;

                intTotalPageCount = Lproducts
                    .Where(x => x.Name.Contains(searchStringUserNameOrEmail) || x.ProductType.Contains(searchStringUserNameOrEmail) || x.SystemType.Contains(searchStringUserNameOrEmail)
                    || x.Processor.Name.Contains(searchStringUserNameOrEmail) || x.VideoCard.Name.Contains(searchStringUserNameOrEmail) || x.Motherboard.Name.Contains(searchStringUserNameOrEmail))
                    .Count();

                var result = Lproducts
                    .Where(x => x.Name.Contains(searchStringUserNameOrEmail) || x.ProductType.Contains(searchStringUserNameOrEmail) || x.SystemType.Contains(searchStringUserNameOrEmail)
                    || x.Processor.Name.Contains(searchStringUserNameOrEmail) || x.VideoCard.Name.Contains(searchStringUserNameOrEmail) || x.Motherboard.Name.Contains(searchStringUserNameOrEmail))
                    .OrderBy(x => x.Name)
                    .Skip(intSkip)
                    .Take(intPageSize)
                    .ToList();

                List<ProductsViewModel> attributionsList = result.Select(x => new ProductsViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    ProductType = x.ProductType,
                    ProcessorName = x.Processor.Name,
                    VideoCardName = x.VideoCard.Name,
                    BottomBoradName = x.Motherboard.Name,
                    SystemType = x.SystemType,
                    Image = x.Image
                }).ToList();

                // Set the number of pages
                var _UserDTOAsIPagedList =
                    new StaticPagedList<ProductsViewModel>
                    (
                        attributionsList, intPage, intPageSize, intTotalPageCount
                        );

                return View(_UserDTOAsIPagedList);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error: " + ex);
                List<ProductsViewModel> col_UserDTO = new List<ProductsViewModel>();

                return View(col_UserDTO.ToPagedList(1, 25));
            }
        }
        public ActionResult InfoProduct(int Id)
        {
            Product product = DB.Product.SingleOrDefault(x => x.Id == Id);
            List<Product> products = DB.Product.Where(x => x.Id == Id).ToList();
            List<Processor> processors = DB.Processor.Where(x => x.Id == product.ProcessorID).ToList();
            List<VideoCard> videoCards = DB.VideoCard.Where(x => x.Id == product.VideoCardID).ToList();
            List<Motherboard> motherboards = DB.Motherboard.Where(x => x.Id == product.BottomBoradID).ToList();

            InfoViewModel model = new InfoViewModel();

            model.ListProducts = products;
            model.ListProcessor = processors;
            model.ListVideoCard = videoCards;
            model.ListMotherboard = motherboards;

            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult AddProduct(int? Id)
        {
            List<Processor> LProcessor = DB.Processor.ToList();
            List<VideoCard> LVideoCard = DB.VideoCard.ToList();
            List<Motherboard> LBottomBoard = DB.Motherboard.ToList();
            if (Id == null)
            {

                ViewBag.BagProcessor = new SelectList(LProcessor, "Id", "Name");
                ViewBag.BagVideoCard = new SelectList(LVideoCard, "Id", "Name");
                ViewBag.BagBottomBoard = new SelectList(LBottomBoard, "Id", "Name");
                return View();
            }
            else
            {
                ProductsViewModel model = new ProductsViewModel();
                Product product = DB.Product.SingleOrDefault(x => x.Id == Id);
                model.Name = product.Name;
                model.ProductType = product.ProductType;
                ViewBag.BagProcessor = new SelectList(LProcessor, "Id", "Name");
                ViewBag.BagVideoCard = new SelectList(LVideoCard, "Id", "Name");
                ViewBag.BagBottomBoard = new SelectList(LBottomBoard, "Id", "Name");
                model.SystemType = product.SystemType;
                return View(model);
            }


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProduct(ProductsViewModel model, int? Id, HttpPostedFileBase file)
        {
            if (Id == null)
            {

                try
                {
                    if (ModelState.IsValid == true)
                    {
                        Product po = new Product();
                        po.Name = model.Name;
                        po.ProductType = model.ProductType;
                        po.ProcessorID = model.ProcessorID;
                        po.VideoCardID = model.VideoCardID;
                        po.BottomBoradID = model.BottomBoradID;
                        po.SystemType = model.SystemType;
                        po.Image = Path.GetFileName(file.FileName);
                        string physicalPath = Path.Combine(Server.MapPath("~/Content/Images"), file.FileName);
                        // save image in folder
                        file.SaveAs(physicalPath);


                        DB.Product.Add(po);
                        DB.SaveChanges();

                        int latestProId = po.Id;
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }

                return RedirectToAction("Index");
            }
            else
            {
                if (ModelState.IsValid == true)
                {
                    Product product = DB.Product.SingleOrDefault(x => x.Id == Id);
                    product.Name = model.Name;
                    product.ProductType = model.ProductType;
                    product.ProcessorID = model.ProcessorID;
                    product.VideoCardID = model.VideoCardID;
                    product.BottomBoradID = model.BottomBoradID;
                    product.SystemType = model.SystemType;
                    product.Image = Path.GetFileName(file.FileName);
                    string physicalPath = Path.Combine(Server.MapPath("~/Content/Images"), file.FileName);
                    // save image in folder
                    file.SaveAs(physicalPath);

                    DB.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }
        public ActionResult ListProcessor()
        {
            List<Processor> LProcessor = DB.Processor.ToList();

            List<ProcessorViewModel> LproductsVM = LProcessor.Select(x => new ProcessorViewModel
            {
                Id = x.Id,
                Name = x.Name,
                PoworGHz = x.PoworGHz,
                Core = x.Core,
                Image = x.Image
            }).ToList();
            return View(LproductsVM);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult AddProcessor(int? Id)
        {
            if (Id == null)
            {
                return View();
            }
            else
            {
                ProcessorViewModel model = new ProcessorViewModel();
                Processor processor = DB.Processor.SingleOrDefault(x => x.Id == Id);
                model.Name = processor.Name;
                model.PoworGHz = processor.PoworGHz;
                model.Core = processor.Core;
                return View(model);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProcessor(ProcessorViewModel model, int? Id, HttpPostedFileBase file)
        {
            if (ModelState.IsValid == true)
            {
                if (Id == null)
                {
                    Processor pr = new Processor();
                    pr.Name = model.Name;
                    pr.PoworGHz = model.PoworGHz;
                    pr.Core = model.Core;
                    pr.Image = Path.GetFileName(file.FileName);
                    string physicalPath = Path.Combine(Server.MapPath("~/Content/Images"), file.FileName);
                    // save image in folder
                    file.SaveAs(physicalPath);
                    DB.Processor.Add(pr);
                    DB.SaveChanges();

                    return View(model);
                }
                else
                {
                    Processor processor = DB.Processor.SingleOrDefault(x => x.Id == Id);
                    processor.Name = model.Name;
                    processor.PoworGHz = model.PoworGHz;
                    processor.Core = model.Core;
                    processor.Image = Path.GetFileName(file.FileName);
                    string physicalPath = Path.Combine(Server.MapPath("~/Content/Images"), file.FileName);
                    // save image in folder
                    file.SaveAs(physicalPath);
                    DB.SaveChanges();

                    return RedirectToAction("ListProcessor");
                }
            }
            return View(model);
        }
        public ActionResult ListVideoCard()
        {
            List<VideoCard> LVideCard = DB.VideoCard.ToList();

            List<VideoCardViewModel> LVideCardVM = LVideCard.Select(x => new VideoCardViewModel
            {
                Id = x.Id,
                Name = x.Name,
                MaxDigitalResolution = x.MaxDigitalResolution,
                MaxVGAResolution = x.MaxVGAResolution,
                MemoryBit = x.MemoryBit,
                MemoryGBsec = x.MemoryGBsec,
                Image = x.Image
            }).ToList();
            return View(LVideCardVM);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult AddVideoCard(int? Id)
        {
            if (Id == null)
            {
                return View();
            }
            else
            {
                VideoCardViewModel model = new VideoCardViewModel();
                VideoCard videoCard = DB.VideoCard.SingleOrDefault(x => x.Id == Id);
                model.Name = videoCard.Name;
                model.MaxDigitalResolution = videoCard.MaxDigitalResolution;
                model.MaxVGAResolution = videoCard.MaxVGAResolution;
                model.MemoryBit = videoCard.MemoryBit;
                model.MemoryGBsec = videoCard.MemoryGBsec;
                return View(model);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddVideoCard(VideoCardViewModel model, int? Id, HttpPostedFileBase file)
        {
            if (ModelState.IsValid == true)
            {
                if (Id == null)
                {
                    VideoCard vc = new VideoCard();
                    vc.Name = model.Name;
                    vc.MaxDigitalResolution = model.MaxDigitalResolution;
                    vc.MaxVGAResolution = model.MaxVGAResolution;
                    vc.MemoryBit = model.MemoryBit;
                    vc.MemoryGBsec = model.MemoryGBsec;
                    vc.Image = Path.GetFileName(file.FileName);
                    string physicalPath = Path.Combine(Server.MapPath("~/Content/Images"), file.FileName);
                    // save image in folder
                    file.SaveAs(physicalPath);
                    DB.VideoCard.Add(vc);
                    DB.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    VideoCard videoCard = DB.VideoCard.SingleOrDefault(x => x.Id == Id);
                    videoCard.Name = model.Name;
                    videoCard.MaxDigitalResolution = model.MaxDigitalResolution;
                    videoCard.MaxVGAResolution = model.MaxVGAResolution;
                    videoCard.MemoryBit = model.MemoryBit;
                    videoCard.MemoryGBsec = model.MemoryGBsec;
                    videoCard.Image = Path.GetFileName(file.FileName);
                    string physicalPath = Path.Combine(Server.MapPath("~/Content/Images"), file.FileName);
                    // save image in folder
                    file.SaveAs(physicalPath);
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }
        public ActionResult ListBottomBoard()
        {
            List<Motherboard> Lmotherboards = DB.Motherboard.ToList();
            List<BottomBoardViewModel> LbottomBoards = Lmotherboards.Select(x => new BottomBoardViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Socket = x.Socket,
                ProcessorSupp = x.ProcessorSupp,
                RAMMemorySupp = x.RAMMemorySupp,
                Image = x.Image
            }).ToList();
            return View(LbottomBoards);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult AddBottomBoard(int? Id)
        {
            if (Id == null)
            {
                return View();
            }
            else
            {
                BottomBoardViewModel model = new BottomBoardViewModel();
                Motherboard motherboard = DB.Motherboard.SingleOrDefault(x => x.Id == Id);
                model.Name = motherboard.Name;
                model.Socket = motherboard.Socket;
                model.ProcessorSupp = motherboard.ProcessorSupp;
                model.RAMMemorySupp = motherboard.RAMMemorySupp;
                return View(model);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddBottomBoard(BottomBoardViewModel model, int? Id, HttpPostedFileBase file)
        {
            if (ModelState.IsValid == true)
            {
                if (Id == null)
                {
                    Motherboard bb = new Motherboard();
                    bb.Name = model.Name;
                    bb.Socket = model.Socket;
                    bb.ProcessorSupp = model.ProcessorSupp;
                    bb.RAMMemorySupp = model.RAMMemorySupp;
                    bb.Image = Path.GetFileName(file.FileName);
                    string physicalPath = Path.Combine(Server.MapPath("~/Content/Images"), file.FileName);
                    // save image in folder
                    file.SaveAs(physicalPath);
                    DB.Motherboard.Add(bb);
                    DB.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    Motherboard motherboard = DB.Motherboard.SingleOrDefault(x => x.Id == Id);
                    motherboard.Name = model.Name;
                    motherboard.Socket = model.Socket;
                    motherboard.ProcessorSupp = model.ProcessorSupp;
                    motherboard.RAMMemorySupp = model.RAMMemorySupp;
                    motherboard.Image = Path.GetFileName(file.FileName);
                    string physicalPath = Path.Combine(Server.MapPath("~/Content/Images"), file.FileName);
                    // save image in folder
                    file.SaveAs(physicalPath);
                    DB.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteProduct(int Id)
        {
            Product product = DB.Product.Find(Id);
            DB.Product.Remove(product);
            DB.SaveChanges();

            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteProcessor(int Id)
        {
            Processor processor = DB.Processor.Find(Id);
            DB.Processor.Remove(processor);
            DB.SaveChanges();

            return RedirectToAction("ListProcessor");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteVideoCard(int Id)
        {
            VideoCard video = DB.VideoCard.Find(Id);
            DB.VideoCard.Remove(video);
            DB.SaveChanges();

            return RedirectToAction("ListVideoCard");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteMotherboard(int Id)
        {
            Motherboard motherboard = DB.Motherboard.Find(Id);
            DB.Motherboard.Remove(motherboard);
            DB.SaveChanges();

            return RedirectToAction("ListBottomBoard");
        }
        [Authorize]
        public ActionResult ListCart()
        {
            var userID = User.Identity.GetUserId();
            Cart cart = DB.Cart.SingleOrDefault(x => x.UserID == userID && x.AcceptedOrder == 0);
            if (cart != null)
            {
                CartViewModel model = new CartViewModel();
                List<Product> products = DB.Product.Where(x => x.Id == cart.PC_ID).ToList();
                List<Processor> processors = DB.Processor.Where(x => x.Id == cart.Processor_ID).ToList();
                List<VideoCard> videoCards = DB.VideoCard.Where(x => x.Id == cart.VideoCard_ID).ToList();
                List<Motherboard> motherboards = DB.Motherboard.Where(x => x.Id == cart.Motherboard_ID).ToList();

                if (products.Count > 0)
                {
                    model.ListProducts = products;
                    model.PC_Orders_Count = cart.PC_Orders_Count;
                }
                if (processors.Count > 0)
                {
                    model.ListProcessor = processors;
                    model.Processor_Order_Cout = cart.Processor_Order_Cout;
                }
                if (videoCards.Count > 0)
                {
                    model.ListVideoCard = videoCards;
                    model.VideoCard_Order_Count = cart.VideoCard_Order_Count;
                }
                if (motherboards.Count > 0)
                {
                    model.ListMotherboard = motherboards;
                    model.Motherboard_Order_Count = cart.Processor_Order_Cout;
                }
                return View(model);
            }
            return RedirectToAction("Index");
        }
        [Authorize]
        public ActionResult CartProducts(ProductsViewModel model)
        {
            var userID = User.Identity.GetUserId();
            Cart product = DB.Cart.SingleOrDefault(x => x.UserID == userID && x.AcceptedOrder == 0);
            if (product == null)
            {
                Cart cart = new Cart();
                cart.UserID = userID;
                cart.PC_ID = model.Id;
                cart.PC_Orders_Count = 1;

                DB.Cart.Add(cart);
                DB.SaveChanges();
            }
            else
            {
                product.UserID = userID;
                product.PC_ID = model.Id;
                if (product.PC_Orders_Count == null)
                {
                    product.PC_Orders_Count = 1;
                }
                else
                {
                    product.PC_Orders_Count += 1;
                }


                DB.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        [Authorize]
        public ActionResult CartVideoCard(VideoCardViewModel model)
        {
            var userID = User.Identity.GetUserId();
            Cart product = DB.Cart.SingleOrDefault(x => x.UserID == userID && x.AcceptedOrder == 0);
            if (product == null)
            {
                Cart cart = new Cart();
                cart.UserID = userID;
                cart.VideoCard_ID = model.Id;


                DB.Cart.Add(cart);
                DB.SaveChanges();
            }
            else
            {
                product.UserID = userID;
                product.VideoCard_ID = model.Id;
                if (product.VideoCard_Order_Count == null)
                {
                    product.VideoCard_Order_Count = 1;
                }
                else
                {
                    product.VideoCard_Order_Count += 1;
                }
                DB.SaveChanges();
            }
            return RedirectToAction("ListVideoCard");
        }
        [Authorize]
        public ActionResult CartProcessor(ProcessorViewModel model)
        {
            var userID = User.Identity.GetUserId();
            Cart product = DB.Cart.SingleOrDefault(x => x.UserID == userID && x.AcceptedOrder == 0);
            if (product == null)
            {
                Cart cart = new Cart();
                cart.UserID = userID;
                cart.Processor_ID = model.Id;
                cart.Processor_Order_Cout = 1;

                DB.Cart.Add(cart);
                DB.SaveChanges();
            }
            else
            {
                product.UserID = userID;
                product.Processor_ID = model.Id;
                if (product.Processor_Order_Cout == null)
                {
                    product.Processor_Order_Cout = 1;
                }
                else
                {
                    product.Processor_Order_Cout += 1;
                }

                DB.SaveChanges();
            }
            return RedirectToAction("ListProcessor");
        }
        [Authorize]
        public ActionResult CartBottomBoard(BottomBoardViewModel model)
        {
            var userID = User.Identity.GetUserId();
            Cart product = DB.Cart.SingleOrDefault(x => x.UserID == userID && x.AcceptedOrder == 0);
            if (product == null)
            {
                Cart cart = new Cart();
                cart.UserID = userID;
                cart.Motherboard_ID = model.Id;
                cart.Motherboard_Order_Count = 1;

                DB.Cart.Add(cart);
                DB.SaveChanges();
            }
            else
            {
                product.UserID = userID;
                product.Motherboard_ID = model.Id;
                product.Motherboard_Order_Count += 1;

                DB.SaveChanges();
            }

            return RedirectToAction("ListBottomBoard");
        }
        public ActionResult ConfirmedOrder()
        {
            var userID = User.Identity.GetUserId();
            Cart product = DB.Cart.SingleOrDefault(x => x.UserID == userID && x.AcceptedOrder == 0);
            if (product != null)
            {
                product.AcceptedOrder = 1;

                DB.SaveChanges();

            }


            return RedirectToAction("ListCart");
        }
    }
}