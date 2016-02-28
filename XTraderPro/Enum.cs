using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;


namespace TradingLib.TraderControl
{
    /// <summary>
    /// X轴坐标位置
    /// </summary>
    public enum EnumScaleAlignment
    {
        Right,
        Left,
    }

    /// <summary>
    /// 制图
    /// </summary>
    public enum EnumScaleType
    { 
        Linear,
        Log,
    
    }

    public enum EnumIndicator
    {
        
        indSimpleMovingAverage = 0,
        indExponentialMovingAverage = indSimpleMovingAverage + 1,
        indTimeSeriesMovingAverage = indExponentialMovingAverage + 1,
        indTriangularMovingAverage = indTimeSeriesMovingAverage + 1,
        indVariableMovingAverage = indTriangularMovingAverage + 1,
        indVIDYA = indVariableMovingAverage + 1,
        indWellesWilderSmoothing = indVIDYA + 1,
        indWeightedMovingAverage = indWellesWilderSmoothing + 1,
        indWilliamsPctR = indWeightedMovingAverage + 1,
        indWilliamsAccumulationDistribution = indWilliamsPctR + 1,
        indVolumeOscillator = indWilliamsAccumulationDistribution + 1,
        indVerticalHorizontalFilter = indVolumeOscillator + 1,
        indUltimateOscillator = indVerticalHorizontalFilter + 1,
        indTrueRange = indUltimateOscillator + 1,
        indTRIX = indTrueRange + 1,
        indRainbowOscillator = indTRIX + 1,
        indPriceOscillator = indRainbowOscillator + 1,
        indParabolicSAR = indPriceOscillator + 1,
        indMomentumOscillator = indParabolicSAR + 1,
        indMACD = indMomentumOscillator + 1,
        indEaseOfMovement = indMACD + 1,
        indDirectionalMovementSystem = indEaseOfMovement + 1,
        indDetrendedPriceOscillator = indDirectionalMovementSystem + 1,
        indChandeMomentumOscillator = indDetrendedPriceOscillator + 1,
        indChaikinVolatility = indChandeMomentumOscillator + 1,
        indAroon = indChaikinVolatility + 1,
        indAroonOscillator = indAroon + 1,
        indLinearRegressionRSquared = indAroonOscillator + 1,
        indLinearRegressionForecast = indLinearRegressionRSquared + 1,
        indLinearRegressionSlope = indLinearRegressionForecast + 1,
        indLinearRegressionIntercept = indLinearRegressionSlope + 1,
        indPriceVolumeTrend = indLinearRegressionIntercept + 1,
        indPerformanceIndex = indPriceVolumeTrend + 1,
        indCommodityChannelIndex = indPerformanceIndex + 1,
        indChaikinMoneyFlow = indCommodityChannelIndex + 1,
        indWeightedClose = indChaikinMoneyFlow + 1,
        indVolumeROC = indWeightedClose + 1,
        indTypicalPrice = indVolumeROC + 1,
        indStandardDeviation = indTypicalPrice + 1,
        indPriceROC = indStandardDeviation + 1,
        indMedian = indPriceROC + 1,
        indHighMinusLow = indMedian + 1,
        [Description("布林带")]
        indBollingerBands = indHighMinusLow + 1,
        indFractalChaosBands = indBollingerBands + 1,
        indHighLowBands = indFractalChaosBands + 1,
        indMovingAverageEnvelope = indHighLowBands + 1,
        indSwingIndex = indMovingAverageEnvelope + 1,
        indAccumulativeSwingIndex = indSwingIndex + 1,
        indComparativeRelativeStrength = indAccumulativeSwingIndex + 1,
        indMassIndex = indComparativeRelativeStrength + 1,
        indMoneyFlowIndex = indMassIndex + 1,
        indNegativeVolumeIndex = indMoneyFlowIndex + 1,
        indOnBalanceVolume = indNegativeVolumeIndex + 1,
        indPositiveVolumeIndex = indOnBalanceVolume + 1,
        indRelativeStrengthIndex = indPositiveVolumeIndex + 1,
        indTradeVolumeIndex = indRelativeStrengthIndex + 1,
        indStochasticOscillator = indTradeVolumeIndex + 1,
        indStochasticMomentumIndex = indStochasticOscillator + 1,
        indFractalChaosOscillator = indStochasticMomentumIndex + 1,
        indPrimeNumberOscillator = indFractalChaosOscillator + 1,
        indPrimeNumberBands = indPrimeNumberOscillator + 1,
        indHistoricalVolatility = indPrimeNumberBands + 1,
        indMACDHistogram = indHistoricalVolatility + 1,
        indElderRayBullPower = indMACDHistogram + 1,
        indElderRayBearPower = indElderRayBullPower + 1,
        indElderForceIndex = indElderRayBearPower + 1,
        indElderThermometer = indElderForceIndex + 1,
        indEhlerFisherTransform = indElderThermometer + 1,
        indKeltnerChannel = indEhlerFisherTransform + 1,
        indMarketFacilitationIndex = indKeltnerChannel + 1,
        indSchaffTrendCycle = indMarketFacilitationIndex + 1,
        indQStick = indSchaffTrendCycle + 1,
        indSTARC = indQStick + 1,
        indCenterOfGravity = indSTARC + 1,
        indCoppockCurve = indCenterOfGravity + 1,
        indChandeForecastOscillator = indCoppockCurve + 1,
        indGopalakrishnanRangeIndex = indChandeForecastOscillator + 1,
        indIntradayMomentumIndex = indGopalakrishnanRangeIndex + 1,
        indKlingerVolumeOscillator = indIntradayMomentumIndex + 1,
        indPrettyGoodOscillator = indKlingerVolumeOscillator + 1,
        indRAVI = indPrettyGoodOscillator + 1,
        indRandomWalkIndex = indRAVI + 1,
        indTwiggsMoneyFlow = indRandomWalkIndex + 1,
        indCustomIndicator = indTwiggsMoneyFlow + 1,
        LastIndicator = indCustomIndicator + 1
    }

}
