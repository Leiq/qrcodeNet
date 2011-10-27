﻿namespace Gma.QrCodeNet.Encoding.VersionControl
{
	internal struct ErrorCorrectionBlock
	{
		internal int NumErrorCorrectionBlock { get; private set;}
		
		internal int NumDataCodewords { get; private set;}
		
		internal ErrorCorrectionBlock(int numErrorCorrectionBlock, int numDataCodewards)
			: this()
		{
			this.NumErrorCorrectionBlock = numErrorCorrectionBlock;
			this.NumDataCodewords = numDataCodewards;
		}
	}
}