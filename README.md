# Prism.CommonDialogPack
Prism の IDialogService を利用した、汎用的なダイアログを提供するライブラリです。</br>
また、Dialog の呼び出し側から Window と Content の Width, Height や WindowStartupLocation, ResizeMode, SizeToContent などの指定をすることが可能になっています。</br>
さらに、呼び出し側では Window の Style も指定することができ、指定した Style が Dialog の Window に Binding される `StyleableDialogService` を提供しています。</br>
`StyleableDialogService` を使用することで、[MahApps.Metro](https://github.com/MahApps/MahApps.Metro)などの通常とは異なる Window を使用する際でも正しく Style が反映されます。

## [Install](https://www.nuget.org/packages/PrismCommonDialogPack/)
nuget パッケージマネージャーコンソールで `Install-Package PrismCommonDialogPack` と入力します。

## Documentation
* [Quick Start](https://github.com/Kuro4/Prism.CommonDialogPack/wiki/Quick-Start)
* [wiki](https://github.com/Kuro4/Prism.CommonDialogPack/wiki)

# 機能

## Notification Dialog

![Norification.png](https://github.com/Kuro4/Prism.CommonDialogPack/blob/master/Images/Norification.png)

## Confirmation Dialog

![Confirmation.png](https://github.com/Kuro4/Prism.CommonDialogPack/blob/master/Images/Confirmation.png)

## FileSelect Dialog
Single select.

![SingleFileSelect.png](https://github.com/Kuro4/Prism.CommonDialogPack/blob/master/Images/SingleFileSelect.png)

Multi select.

![MultiFileSelect.png](https://github.com/Kuro4/Prism.CommonDialogPack/blob/master/Images/MultiFileSelect.png)

## FolderSelect Dialog
Single select.

![SingleFolderSelect.png](https://github.com/Kuro4/Prism.CommonDialogPack/blob/master/Images/SingleFolderSelect.png)

Multi select.

![MultiFolderSelect.png](https://github.com/Kuro4/Prism.CommonDialogPack/blob/master/Images/MultiFolderSelect.png)

## FileSave Dialog

![FileSave.png](https://github.com/Kuro4/Prism.CommonDialogPack/blob/master/Images/FileSave.png)

## アイコンなどのカスタマイズ

![Customize.png](https://github.com/Kuro4/Prism.CommonDialogPack/blob/master/Images/Customize.png)

## Progress Dialog
![ProgressDemo.gif](https://github.com/Kuro4/Prism.CommonDialogPack/blob/master/Images/ProgressDemo.gif)

## ColorPicker Dialog
![ColorPicker.jpeg](https://github.com/Kuro4/Prism.CommonDialogPack/blob/master/Images/ColorPicker.jpeg)

## DialogWindow のカスタマイズ

![MahApps.png](https://github.com/Kuro4/Prism.CommonDialogPack/blob/master/Images/MahApps.png)
