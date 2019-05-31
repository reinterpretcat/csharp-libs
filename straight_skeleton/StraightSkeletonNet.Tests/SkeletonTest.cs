using System.Collections.Generic;
using NUnit.Framework;
using StraightSkeletonNet.Primitives;

namespace StraightSkeletonNet.Tests
{
    [TestFixture]
    internal class SkeletonTest
    {
        [Test]
        public void CircularAddTest()
        {
            var polygon = new List<Vector2d>
            {
                new Vector2d(50, 50),
                new Vector2d(100, 50),
                new Vector2d(100, 100),
                new Vector2d(50, 100)
            };

            var expected = new List<Vector2d> {new Vector2d(75.000000, 75.000000)};
            expected.AddRange(polygon);

            var sk = SkeletonBuilder.Build(polygon);

            SkeletonTestUtil.AssertExpectedPoints(expected, SkeletonTestUtil.GetFacePoints(sk));
        }

        [Test]
        public void CircularAddTest2()
        {
            var polygon = new List<Vector2d>
            {
                new Vector2d(50, 50),
                new Vector2d(150, 50),
                new Vector2d(150, 100),
                new Vector2d(50, 100)
            };

            var expected = new List<Vector2d>
            {
                new Vector2d(75.000000, 75.000000),
                new Vector2d(125.000000, 75.000000)
            };
            expected.AddRange(polygon);

            var sk = SkeletonBuilder.Build(polygon);


            SkeletonTestUtil.AssertExpectedPoints(expected, SkeletonTestUtil.GetFacePoints(sk));
        }

        [Test]
        public void SkeletonTest_hole_1()
        {
            var inner = new List<Vector2d>
            {
                new Vector2d(-1, 1),
                new Vector2d(1, 1),
                new Vector2d(1, -1),
                new Vector2d(-1, -1)
            };

            var outer = new List<Vector2d>
            {
                new Vector2d(-2, -2),
                new Vector2d(2, -2),
                new Vector2d(2, 2),
                new Vector2d(-2, 2)
            };

            var innerList = new List<List<Vector2d>> {inner};

            var expected = new List<Vector2d>
            {
                new Vector2d(-1.500000, -1.500000),
                new Vector2d(-1.500000, 1.500000),
                new Vector2d(1.500000, -1.500000),
                new Vector2d(1.500000, 1.500000)
            };
            expected.AddRange(outer);
            expected.AddRange(inner);


            var sk = SkeletonBuilder.Build(outer, innerList);


            SkeletonTestUtil.AssertExpectedPoints(expected, SkeletonTestUtil.GetFacePoints(sk));
        }

        [Test]
        public void SkeletonTest_hole_2()
        {
            var polygon = new List<Vector2d>
            {
                new Vector2d(0.0, 0.0),
                new Vector2d(7.087653026630875, -0.0572739636795121),
                new Vector2d(7.035244566479503, -6.5428208800475005),
                new Vector2d(-0.052408459722688594, -6.485546915224834)
            };

            var hole = new List<Vector2d>
            {
                new Vector2d(1.4849939588531493, -1.5250224044562133),
                new Vector2d(1.4341762422598874, -5.1814705083480606),
                new Vector2d(5.747532319228888, -5.241418004618678),
                new Vector2d(5.798350035536362, -1.5849699030131408)
            };

            var innerList = new List<List<Vector2d>> {hole};

            var expected = new List<Vector2d>();
            expected.AddRange(polygon);
            expected.AddRange(hole);

            expected.Add(new Vector2d(6.3821371859978875, -5.893911100019249));
            expected.Add(new Vector2d(0.7651208111455217, -5.8321836510415475));
            expected.Add(new Vector2d(0.6898242249025952, -5.755213752675646));
            expected.Add(new Vector2d(6.389576876981116, -5.886633146615758));
            expected.Add(new Vector2d(6.443747494495353, -0.9572661447277495));
            expected.Add(new Vector2d(6.310953658294117, -0.8215212379272131));
            expected.Add(new Vector2d(0.7481994722534444, -0.7603900949775717));
            expected.Add(new Vector2d(0.7446762937827887, -0.7638366801629576));

            var sk = SkeletonBuilder.Build(polygon, innerList);


            SkeletonTestUtil.AssertExpectedPoints(expected, SkeletonTestUtil.GetFacePoints(sk));
        }

        [Test]
        public void SkeletonTest5()
        {
            var polygon = new List<Vector2d>
            {
                new Vector2d(-2, 0),
                new Vector2d(-1, -1),
                new Vector2d(0, 0),
                new Vector2d(1, -1),
                new Vector2d(2, 0),
                new Vector2d(1, 1),
                new Vector2d(-1, 1)
            };

            var expected = new List<Vector2d>
            {
                new Vector2d(-1.000000, 0.000000),
                new Vector2d(-0.707107, 0.292893),
                new Vector2d(0.000000, 0.585786),
                new Vector2d(0.707107, 0.292893),
                new Vector2d(1.000000, 0.000000)
            };
            expected.AddRange(polygon);

            var sk = SkeletonBuilder.Build(polygon);

            SkeletonTestUtil.AssertExpectedPoints(expected, SkeletonTestUtil.GetFacePoints(sk));
        }

        [Test]
        public void SkeletonTest6_9()
        {
            var polygon = new List<Vector2d>
            {
                new Vector2d(119, 158),
                new Vector2d(259, 159),
                new Vector2d(248, 63),
                new Vector2d(126, 60),
                new Vector2d(90, 106)
            };

            var expected = new List<Vector2d>
            {
                new Vector2d(147.156672, 110.447627),
                new Vector2d(149.322770, 109.401806),
                new Vector2d(204.771536, 110.281518)
            };
            expected.AddRange(polygon);

            var sk = SkeletonBuilder.Build(polygon);


            SkeletonTestUtil.AssertExpectedPoints(expected, SkeletonTestUtil.GetFacePoints(sk));
        }

        [Test]
        public void SkeletonTest7()
        {
            var polygon = new List<Vector2d>
            {
                new Vector2d(0, 0),
                new Vector2d(0, -1),
                new Vector2d(1, -1),
                new Vector2d(1, 1),
                new Vector2d(-1, 1)
            };

            var expected = new List<Vector2d>
            {
                new Vector2d(0.414214, 0.414214),
                new Vector2d(0.500000, -0.500000),
                new Vector2d(0.500000, 0.207107)
            };
            expected.AddRange(polygon);


            var sk = SkeletonBuilder.Build(polygon);


            SkeletonTestUtil.AssertExpectedPoints(expected, SkeletonTestUtil.GetFacePoints(sk));
        }

        [Test]
        public void SkeletonTest8()
        {
            var polygon = new List<Vector2d>
            {
                new Vector2d(-1, 0),
                new Vector2d(-1.2, -2),
                new Vector2d(1.2, -2),
                new Vector2d(1, 0.5),
                new Vector2d(2, -0.2),
                new Vector2d(2, 1),
                new Vector2d(-2, 1.2),
                new Vector2d(-2, -0.2)
            };


            var expected = new List<Vector2d>
            {
                new Vector2d(-1.383546, 0.551953),
                new Vector2d(-0.436065, 0.621927),
                new Vector2d(0.011951, -0.903199),
                new Vector2d(0.021802, 0.089862),
                new Vector2d(0.784764, 0.875962),
                new Vector2d(1.582159, 0.602529)
            };
            expected.AddRange(polygon);


            var sk = SkeletonBuilder.Build(polygon);


            SkeletonTestUtil.AssertExpectedPoints(expected, SkeletonTestUtil.GetFacePoints(sk));
        }

        [Test]
        public void SkeletonTest9()
        {
            var polygon = new List<Vector2d>
            {
                new Vector2d(77, 85),
                new Vector2d(198, 85),
                new Vector2d(196, 139),
                new Vector2d(150, 119),
                new Vector2d(157, 177),
                new Vector2d(112, 179),
                new Vector2d(125, 130),
                new Vector2d(68, 118)
            };


            var expected = new List<Vector2d>
            {
                new Vector2d(91.320644, 103.749308),
                new Vector2d(126.066597, 107.367125),
                new Vector2d(134.360696, 98.011826),
                new Vector2d(136.287191, 159.442502),
                new Vector2d(138.938550, 121.416104),
                new Vector2d(175.597143, 106.588481)
            };
            expected.AddRange(polygon);


            var sk = SkeletonBuilder.Build(polygon);


            SkeletonTestUtil.AssertExpectedPoints(expected, SkeletonTestUtil.GetFacePoints(sk));
        }

        [Test]
        public void SkeletonTestB1()
        {
            var polygon = new List<Vector2d>
            {
                new Vector2d(0.0, 0.0),
                new Vector2d(0.7904833761575505, 8.520486967634694),
                new Vector2d(5.978418789681697, 8.712497973454056),
                new Vector2d(5.95269105167549, -2.6355979260267777),
                new Vector2d(4.566910029680516, -2.6324561649763485),
                new Vector2d(4.5603585630377115, -5.522203838861205),
                new Vector2d(6.043569207647302, -5.525566487736131),
                new Vector2d(6.038049999411376, -7.960001358506733),
                new Vector2d(9.886846028372108, -7.968727126586532),
                new Vector2d(9.902081573281308, -1.248570683335708),
                new Vector2d(13.742215004880482, -1.2572768087753285),
                new Vector2d(13.75400717659087, 3.9440624000165103),
                new Vector2d(9.194585721152315, 3.9543992526769878),
                new Vector2d(5.823717342947504, 17.30434988614582),
                new Vector2d(5.808494957384097, 10.589997844496661),
                new Vector2d(-0.13214359029800526, 10.603466113057067)
            };


            var expected = new List<Vector2d>
            {
                new Vector2d(0.359453, 8.976136),
                new Vector2d(0.918471, 9.563508),
                new Vector2d(6.008508, -4.080606),
                new Vector2d(6.729881, 9.664425),
                new Vector2d(6.760642, 9.696273),
                new Vector2d(6.858071, 9.623241),
                new Vector2d(7.394289, -4.083747),
                new Vector2d(7.779411, 2.141718),
                new Vector2d(7.923726, -3.556706),
                new Vector2d(7.933442, 0.729015),
                new Vector2d(7.966811, -6.039966),
                new Vector2d(7.972330, -3.605531),
                new Vector2d(8.562422, 1.355149),
                new Vector2d(11.147441, 1.349289)
            };
            expected.AddRange(polygon);


            var sk = SkeletonBuilder.Build(polygon);


            SkeletonTestUtil.AssertExpectedPoints(expected, SkeletonTestUtil.GetFacePoints(sk));
        }

        [Test]
        public void SkeletonTestB10()
        {
            var polygon = new List<Vector2d>
            {
                new Vector2d(0.0, 0.0),
                new Vector2d(23.542862199718826, -1.0957017437087124),
                new Vector2d(12.89581137652037, 1.5573908447103584),
                new Vector2d(13.68678342709616, 5.195862274901293),
                new Vector2d(30.92997412599037, 6.619611963708646),
                new Vector2d(16.53428280871175, 7.568778425199767),
                new Vector2d(13.05400578686415, 8.676139297892002),
                new Vector2d(-4.188927083681472, 7.336703572978552),
                new Vector2d(10.196014852102863, 4.475707108744242),
                new Vector2d(8.782756714583655, 1.5573908412810287)
            };


            var expected = new List<Vector2d>
            {
                new Vector2d(9.496922, 0.613365),
                new Vector2d(10.882442, 1.437594),
                new Vector2d(11.471020, 0.671521),
                new Vector2d(11.720280, 6.390569),
                new Vector2d(12.241556, 6.845124),
                new Vector2d(12.291810, 5.518617),
                new Vector2d(12.847638, 6.893686),
                new Vector2d(16.331903, 6.498860)
            };
            expected.AddRange(polygon);


            var sk = SkeletonBuilder.Build(polygon);


            SkeletonTestUtil.AssertExpectedPoints(expected, SkeletonTestUtil.GetFacePoints(sk));
        }

        [Test]
        public void SkeletonTestB11()
        {
            var polygon = new List<Vector2d>
            {
                new Vector2d(0.0, 0.0),
                new Vector2d(-0.2885918221241157, 14.001053106358517),
                new Vector2d(4.899343591400031, 14.19306411217788),
                new Vector2d(4.873615853393824, 2.8449682126970464),
                new Vector2d(3.4878348313988496, 2.8481099737474747),
                new Vector2d(3.4812833647560453, -0.04163770013738066),
                new Vector2d(4.964494009365636, -0.04500034901230876),
                new Vector2d(4.95897480112971, -2.4794352197829106),
                new Vector2d(8.807770830090442, -2.4881609878627096),
                new Vector2d(8.823006374999641, 4.231995455388115),
                new Vector2d(12.663139806598815, 4.223289329948495),
                new Vector2d(12.674931978309203, 9.424628538740333),
                new Vector2d(8.115510522870647, 9.43496539140081),
                new Vector2d(4.744642144665839, 22.784916024869645),
                new Vector2d(4.729419759102431, 16.070563983220485),
                new Vector2d(-1.2112187885796715, 16.08403225178089)
            };


            var expected = new List<Vector2d>
            {
                new Vector2d(-0.689093, 14.379124),
                new Vector2d(-0.093795, 15.045234),
                new Vector2d(4.929433, 1.399960),
                new Vector2d(5.650806, 15.144991),
                new Vector2d(5.681567, 15.176839),
                new Vector2d(5.778996, 15.103807),
                new Vector2d(6.315214, 1.396819),
                new Vector2d(6.700336, 7.622285),
                new Vector2d(6.844651, 1.923860),
                new Vector2d(6.854367, 6.209582),
                new Vector2d(6.887736, -0.559400),
                new Vector2d(6.893255, 1.875035),
                new Vector2d(7.483346, 6.835716),
                new Vector2d(10.068366, 6.829855)
            };
            expected.AddRange(polygon);


            var sk = SkeletonBuilder.Build(polygon);


            SkeletonTestUtil.AssertExpectedPoints(expected, SkeletonTestUtil.GetFacePoints(sk));
        }

        [Test]
        public void SkeletonTestB11_b()
        {
            var polygon = new List<Vector2d>
            {
                new Vector2d(4.899343591400031, 14.19306411217788),
                new Vector2d(4.873615853393824, 2.8449682126970464),
                new Vector2d(3.4878348313988496, 2.8481099737474747),
                new Vector2d(3.4812833647560453, -0.04163770013738066),
                new Vector2d(4.964494009365636, -0.04500034901230876),
                new Vector2d(4.95897480112971, -2.4794352197829106),
                new Vector2d(8.807770830090442, -2.4881609878627096),
                new Vector2d(8.823006374999641, 4.231995455388115)
            };


            var expected = new List<Vector2d>(polygon)
            {
                new Vector2d(6.8490390285892975, 3.8595532917064257),
                new Vector2d(6.315213958119228, 1.396818641879405),
                new Vector2d(6.844650538271922, 1.9238600368574004),
                new Vector2d(4.929432935568722, 1.3999604034575501),
                new Vector2d(6.893254906247293, 1.875034782130368),
                new Vector2d(6.8877356980830235, -0.5594000893968922)
            };



            var sk = SkeletonBuilder.Build(polygon);


            SkeletonTestUtil.AssertExpectedPoints(expected, SkeletonTestUtil.GetFacePoints(sk));
        }

        [Test]
        public void SkeletonTestB12()
        {
            var polygon = new List<Vector2d>
            {
                new Vector2d(0.0, 0.0),
                new Vector2d(1.6082838074612242, 15.395815413439262),
                new Vector2d(6.796219518140479, 15.587826427398873),
                new Vector2d(6.7704917786606345, 4.239729879063727),
                new Vector2d(5.384710677004972, 4.2428716408656655),
                new Vector2d(5.37815921027269, 1.3531237986037645),
                new Vector2d(6.861369940123552, 1.3497611512508971),
                new Vector2d(6.855850731428608, -1.084673859531076),
                new Vector2d(10.704646980698193, -1.093399628682226),
                new Vector2d(10.719882526622944, 5.626757200629533),
                new Vector2d(14.560016178034793, 5.6180510758343525),
                new Vector2d(14.571808350563504, 10.819390581977487),
                new Vector2d(10.01238663382704, 10.829727434086928),
                new Vector2d(6.64151806240239, 24.179678832787182),
                new Vector2d(6.626295676252851, 17.465326408838887),
                new Vector2d(0.6856567883022331, 17.478794675312955)
            };


            var expected = new List<Vector2d>
            {
                new Vector2d(1.140824, 15.895738),
                new Vector2d(1.684220, 16.437933),
                new Vector2d(6.826309, 2.794722),
                new Vector2d(7.547682, 16.539753),
                new Vector2d(7.578443, 16.571601),
                new Vector2d(7.675872, 16.498570),
                new Vector2d(8.212090, 2.791580),
                new Vector2d(8.597212, 9.017047),
                new Vector2d(8.741527, 3.318622),
                new Vector2d(8.751243, 7.604343),
                new Vector2d(8.784612, 0.835361),
                new Vector2d(8.790131, 3.269796),
                new Vector2d(9.380222, 8.230478),
                new Vector2d(11.965243, 8.224617)
            };
            expected.AddRange(polygon);


            var sk = SkeletonBuilder.Build(polygon);


            SkeletonTestUtil.AssertExpectedPoints(expected, SkeletonTestUtil.GetFacePoints(sk));
        }

        [Test]
        public void SkeletonTestB13()
        {
            var polygon = new List<Vector2d>
            {
                new Vector2d(0.0, 0.0),
                new Vector2d(-0.03697835689094475, 17.903291653889664),
                new Vector2d(9.36122931562474, 17.922703185404146),
                new Vector2d(9.399539490923859, -0.6253964219022965),
                new Vector2d(6.897780217346079, -0.6305636811510293),
                new Vector2d(6.907305814387495, -5.242438102429183),
                new Vector2d(9.496043768204736, -5.2367356072030695),
                new Vector2d(9.673537498409361, -7.819464124646299),
                new Vector2d(19.728934851080233, -7.7986952031890375),
                new Vector2d(19.715280237589244, -1.1877328304801722),
                new Vector2d(23.581205989632387, -1.1797479507986637),
                new Vector2d(23.570459756724986, 4.023104657038741),
                new Vector2d(19.065027189523686, 4.01379891209519),
                new Vector2d(19.009685241927738, 30.807932065847332),
                new Vector2d(9.439383865135643, 30.78816508512935),
                new Vector2d(9.453189359125524, 24.10415305431124),
                new Vector2d(-0.01730198014624129, 24.08459222736407),
                new Vector2d(-0.030597953439544412, 30.521916694234474),
                new Vector2d(-10.417861267451112, 30.500462317733504),
                new Vector2d(-10.354819907553885, -0.021387367337700525)
            };

            var expected = new List<Vector2d>
            {
                new Vector2d(-5.225081993006608, 23.070007924404237),
                new Vector2d(-5.213502422879821, 25.317557848847482),
                new Vector2d(-5.208893794753686, 23.086263132900537),
                new Vector2d(-5.188103636084189, 5.166716270119771),
                new Vector2d(-3.1015352470932616, 20.98759193064646),
                new Vector2d(9.208321529781248, -2.9315800507494063),
                new Vector2d(11.648322280432005, -2.9263727729378277),
                new Vector2d(12.445462580412869, 21.019703480686516),
                new Vector2d(12.606101682729628, -3.818739927261688),
                new Vector2d(13.428106603203808, -3.789677802721639),
                new Vector2d(14.19596815545603, 19.27641416469292),
                new Vector2d(14.234418043971877, 26.012897887101527),
                new Vector2d(14.237504608711998, -0.83370695637133),
                new Vector2d(14.248223537950237, 19.328885855734843),
                new Vector2d(14.557918451002058, -1.1527999293121498),
                new Vector2d(14.561015138561665, -2.652079649029039),
                new Vector2d(17.108480813881517, 1.4083203585579516),
                new Vector2d(20.974406567920894, 1.4163052362523167)
            };

            expected.AddRange(polygon);

            var sk = SkeletonBuilder.Build(polygon);


            SkeletonTestUtil.AssertExpectedPoints(expected, SkeletonTestUtil.GetFacePoints(sk));
        }

        [Test]
        public void SkeletonTestB2()
        {
            var polygon = new List<Vector2d>
            {
                new Vector2d(0.0, 0.0),
                new Vector2d(0.7904833761549828, 8.520486967607015),
                new Vector2d(5.9784187896622765, 8.712497973425755),
                new Vector2d(5.952691051656153, -2.6355979260182156),
                new Vector2d(4.56691002966568, -2.632456164967797),
                new Vector2d(4.560358563022897, -5.522203838843264),
                new Vector2d(6.0435692076276695, -5.525566487718182),
                new Vector2d(6.038049999391761, -7.960001358480875),
                new Vector2d(9.886846028339992, -7.968727126560646),
                new Vector2d(9.902081573249141, -1.2485706833316517),
                new Vector2d(13.74221500483584, -1.2572768087712447),
                new Vector2d(13.754007176546189, 3.944062400003698),
                new Vector2d(9.194585721122445, 3.9543992526641416),
                new Vector2d(9.840828592998651, 10.391220834155359),
                new Vector2d(-0.24573045314637643, 10.433085818392197)
            };


            var expected = new List<Vector2d>
            {
                new Vector2d(0.311377, 9.026957),
                new Vector2d(0.732142, 9.474000),
                new Vector2d(6.008508, -4.080606),
                new Vector2d(6.810341, 9.573824),
                new Vector2d(7.394289, -4.083747),
                new Vector2d(7.498680, 2.423725),
                new Vector2d(7.813149, 8.564560),
                new Vector2d(7.923726, -3.556706),
                new Vector2d(7.933442, 0.729015),
                new Vector2d(7.966811, -6.039966),
                new Vector2d(7.972330, -3.605531),
                new Vector2d(8.562422, 1.355149),
                new Vector2d(11.147441, 1.349289)
            };
            expected.AddRange(polygon);


            var sk = SkeletonBuilder.Build(polygon);


            SkeletonTestUtil.AssertExpectedPoints(expected, SkeletonTestUtil.GetFacePoints(sk));
        }

        [Test]
        public void SkeletonTestB3__()
        {
            var polygon = new List<Vector2d>
            {
                new Vector2d(0.0, 0.0),
                new Vector2d(0.0853589477356087, -5.32440343246266),
                new Vector2d(3.934154976683839, -5.33312920054243),
                new Vector2d(3.9493905215929885, 1.387027242686564),
                new Vector2d(7.789523953179687, 1.378321117246971),
                new Vector2d(3.2418946694662925, 6.589997178682357),
                new Vector2d(-0.4480081827933864, 6.565094698194268)
            };


            var expected = new List<Vector2d>
            {
                new Vector2d(1.860585, 3.485326),
                new Vector2d(1.972676, 0.083065),
                new Vector2d(1.996554, -3.386722),
                new Vector2d(2.146278, 4.158152),
                new Vector2d(2.251879, 3.903281)
            };
            expected.AddRange(polygon);


            var sk = SkeletonBuilder.Build(polygon);


            SkeletonTestUtil.AssertExpectedPoints(expected, SkeletonTestUtil.GetFacePoints(sk));
        }

        [Test]
        public void SkeletonTestB4__()
        {
            var polygon = new List<Vector2d>
            {
                new Vector2d(0.0, 0.0),
                new Vector2d(-1.192493260706565, -5.6367673060470285),
                new Vector2d(2.656302768241665, -5.645493074126799),
                new Vector2d(6.511671744737513, 1.0659572436626021),
                new Vector2d(-1.7258603912355601, 6.252730824609899)
            };

            var expected = new List<Vector2d>
            {
                new Vector2d(1.427912, -3.517645),
                new Vector2d(2.804480, 0.085324),
                new Vector2d(2.812173, 0.146026)
            };
            expected.AddRange(polygon);


            var sk = SkeletonBuilder.Build(polygon);


            SkeletonTestUtil.AssertExpectedPoints(expected, SkeletonTestUtil.GetFacePoints(sk));
        }

        [Test]
        public void SkeletonTestB5__()
        {
            var polygon = new List<Vector2d>
            {
                new Vector2d(0.0, 0.0),
                new Vector2d(-1.192493260706565, -5.6367673060470285),
                new Vector2d(2.656302768241665, -5.645493074126799),
                new Vector2d(7.051209343876594, 2.9401404828825903),
                new Vector2d(-1.7258603912355601, 6.252730824609899)
            };

            var expected = new List<Vector2d>
            {
                new Vector2d(1.381369, -3.555284),
                new Vector2d(2.671019, 0.081263),
                new Vector2d(2.795365, 1.297294)
            };
            expected.AddRange(polygon);


            var sk = SkeletonBuilder.Build(polygon);


            SkeletonTestUtil.AssertExpectedPoints(expected, SkeletonTestUtil.GetFacePoints(sk));
        }

        [Test]
        public void SkeletonTestB6__()
        {
            var polygon = new List<Vector2d>
            {
                new Vector2d(0.0, 0.0),
                new Vector2d(-1.192493260706565, -5.636767306047028),
                new Vector2d(2.656302768241665, -5.645493074126798),
                new Vector2d(5.716563703938576, 6.120572646649897),
                new Vector2d(-5.985367752852362, 6.423111118668768),
                new Vector2d(-6.297731626436729, -3.6293262553813097),
                new Vector2d(-3.4580600517873807, 1.3968924313579514)
            };


            var expected = new List<Vector2d>
            {
                new Vector2d(-4.254893, 3.676216),
                new Vector2d(-3.720036, 4.025044),
                new Vector2d(1.173593, -3.723313),
                new Vector2d(1.493460, 2.941709),
                new Vector2d(2.345444, 1.248630)
            };
            expected.AddRange(polygon);


            var sk = SkeletonBuilder.Build(polygon);


            SkeletonTestUtil.AssertExpectedPoints(expected, SkeletonTestUtil.GetFacePoints(sk));
        }

        [Test]
        public void SkeletonTestB7__()
        {
            var polygon = new List<Vector2d>
            {
                new Vector2d(0.0, 0.0),
                new Vector2d(-1.1889778921584675, -7.356451670462243),
                new Vector2d(5.7257149714503175, -12.035132476438635),
                new Vector2d(11.739705976732338, -17.194940549920428),
                new Vector2d(0.8357970425329011, -1.0288592710693223),
                new Vector2d(7.360455718922119, -6.229013606285628)
            };

            var expected = new List<Vector2d>
            {
                new Vector2d(0.159929, -0.432595),
                new Vector2d(0.228431, -0.371176),
                new Vector2d(1.434035, -6.223122),
                new Vector2d(6.380715, -11.177062)
            };
            expected.AddRange(polygon);


            var sk = SkeletonBuilder.Build(polygon);


            SkeletonTestUtil.AssertExpectedPoints(expected, SkeletonTestUtil.GetFacePoints(sk));
        }

        [Test]
        public void SkeletonTestB8__()
        {
            var polygon = new List<Vector2d>
            {
                new Vector2d(0.0, 0.0),
                new Vector2d(-1.1889778921584675, -7.356451670462243),
                new Vector2d(5.7257149714503175, -12.035132476438635),
                new Vector2d(11.739705976732338, -17.194940549920428),
                new Vector2d(0.8357970425329011, -1.0288592710693223)
            };
            var expected = new List<Vector2d>
            {
                new Vector2d(0.367496, -1.375942),
                new Vector2d(1.434035, -6.223122),
                new Vector2d(6.380715, -11.177062)
            };
            expected.AddRange(polygon);


            var sk = SkeletonBuilder.Build(polygon);


            SkeletonTestUtil.AssertExpectedPoints(expected, SkeletonTestUtil.GetFacePoints(sk));
        }

        [Test]
        public void SkeletonTestC1()
        {
            var polygon = new List<Vector2d>
            {
                new Vector2d(80.8806, -8.9725),
                new Vector2d(81.0041, -8.8169),
                new Vector2d(56.051, 72.1197),
                new Vector2d(17.2096, 0.9978),
                new Vector2d(16.475, -0.3011),
                new Vector2d(15.0244, 0.2603),
                new Vector2d(23.9762, 39.6003),
                new Vector2d(-18.0879, 10.6703),
                new Vector2d(-19.1437, 9.961),
                new Vector2d(-20.173, 11.2989),
                new Vector2d(8.0439, 36.0416),
                new Vector2d(8.0445, 36.0529),
                new Vector2d(23.813, 64.5547),
                new Vector2d(-0.7203, 71.8831),
                new Vector2d(-0.7564, 71.8911),
                new Vector2d(-68.8682, 48.1444),
                new Vector2d(-4.8991, 53.0675),
                new Vector2d(-3.5566, 53.1622),
                new Vector2d(-3.0501, 51.9918),
                new Vector2d(-46.0636, 12.2109),
                new Vector2d(13.8633, -67.41)
            };

            // just check that this runs without throwing an exception
            var sk = SkeletonBuilder.Build(polygon);
        }
    }
}