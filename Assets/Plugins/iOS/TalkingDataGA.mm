//
//  TalkingDataGA.mm
//  TalkingData
//
//  Created by Biao Hou on 12-6-21.
//  Copyright (c) 2012年 TendCloud. All rights reserved.
//

#import "TalkingDataGA.h"

//#define TDGA_CUSTOM     // 自定义事件
//#define TDGA_PUSH       // 推送营销

// Converts C style string to NSString
static NSString *tdgaCreateNSString(const char *string) {
    return string ? [NSString stringWithUTF8String:string] : nil;
}

static TDGAAccount *tdgaAccount = nil;
static char *tdgaDeviceId = NULL;

extern "C" {
#pragma GCC diagnostic ignored "-Wmissing-prototypes"

const char *tdgaGetDeviceId() {
    if (!tdgaDeviceId) {
        NSString *deviceId = [TalkingDataGA getDeviceId];
        tdgaDeviceId = (char *)calloc(deviceId.length + 1, sizeof(char));
        strcpy(tdgaDeviceId, deviceId.UTF8String);
    }
    
    return tdgaDeviceId;
}

void tdgaSetVerboseLogDisabled() {
    [TalkingDataGA setVerboseLogDisabled];
}

void tdgaOnStart(const char *appId, const char *channelId) {
    if ([TalkingDataGA respondsToSelector:@selector(setFrameworkTag:)]) {
        [TalkingDataGA performSelector:@selector(setFrameworkTag:) withObject:@2];
    }
    [TalkingDataGA onStart:tdgaCreateNSString(appId) withChannelId:tdgaCreateNSString(channelId)];
}

void tdgaSetAccount(const char *accountId) {
    tdgaAccount = [TDGAAccount setAccount:tdgaCreateNSString(accountId)];
}

void tdgaSetAccountName(const char *accountName) {
    if (nil != tdgaAccount) {
        [tdgaAccount setAccountName:tdgaCreateNSString(accountName)];
    }
}

void tdgaSetAccountType(int accountType) {
    if (nil != tdgaAccount) {
        [tdgaAccount setAccountType:(TDGAAccountType)accountType];
    }
}

void tdgaSetLevel(int level) {
    if (nil != tdgaAccount) {
        [tdgaAccount setLevel:level];
    }
}

void tdgaSetGender(int gender) {
    if (nil != tdgaAccount) {
        [tdgaAccount setGender:(TDGAGender)gender];
    }
}

void tdgaSetAge(int age) {
    if (nil != tdgaAccount) {
        [tdgaAccount setAge:age];
    }
}

void tdgaSetGameServer(const char *gameServer) {
    if (nil != tdgaAccount) {
        [tdgaAccount setGameServer:tdgaCreateNSString(gameServer)];
    }
}

void tdgaOnBegin(const char *missionId) {
    [TDGAMission onBegin:tdgaCreateNSString(missionId)];
}

void tdgaOnCompleted(const char *missionId) {
    [TDGAMission onCompleted:tdgaCreateNSString(missionId)];
}

void tdgaOnFailed(const char *missionId, const char *failedCause) {
    [TDGAMission onFailed:tdgaCreateNSString(missionId) failedCause:tdgaCreateNSString(failedCause)];
}

void tdgaOnChargeRequst(const char *orderId, const char *iapId, double currencyAmount, const char *currencyType, double virtualCurrencyAmount, const char *paymentType) {
    [TDGAVirtualCurrency onChargeRequst:tdgaCreateNSString(orderId)
                                  iapId:tdgaCreateNSString(iapId)
                         currencyAmount:currencyAmount
                           currencyType:tdgaCreateNSString(currencyType)
                  virtualCurrencyAmount:virtualCurrencyAmount
                            paymentType:tdgaCreateNSString(paymentType)];
}

void tdgaOnChargSuccess(const char *orderId) {
    [TDGAVirtualCurrency onChargeSuccess:tdgaCreateNSString(orderId)];
}

void tdgaOnReward(double virtualCurrencyAmount, const char *reason) {
    [TDGAVirtualCurrency onReward:virtualCurrencyAmount reason:tdgaCreateNSString(reason)];
}

void tdgaOnPurchase(const char *item, int itemNumber, double priceInVirtualCurrency) {
    [TDGAItem onPurchase:tdgaCreateNSString(item) itemNumber:itemNumber priceInVirtualCurrency:priceInVirtualCurrency];
}

void tdgaOnUse(const char *item, int itemNumber) {
    [TDGAItem onUse:tdgaCreateNSString(item) itemNumber:itemNumber];
}

#ifdef TDGA_CUSTOM
void tdgaOnEvent(const char *eventId, const char *parameters) {
    NSString *parameterStr = tdgaCreateNSString(parameters);
    NSData *parameterData = [parameterStr dataUsingEncoding:NSUTF8StringEncoding];
    NSDictionary *parameterDic = [NSJSONSerialization JSONObjectWithData:parameterData options:0 error:nil];
    [TalkingDataGA onEvent:tdgaCreateNSString(eventId) eventData:parameterDic];
}
#endif

void tdgaSetLocation(double latitude, double longitude) {
    [TalkingDataGA setLatitude:latitude longitude:longitude];
}

#ifdef TDGA_PUSH
void tdgaSetDeviceToken(const void *deviceToken, int length) {
    NSData *tokenData = [NSData dataWithBytes:deviceToken length:length];
    [TalkingDataGA setDeviceToken:tokenData];
}

void tdgaHandlePushMessage(const char *message) {
    NSString *val = tdgaCreateNSString(message);
    NSDictionary *dic = [NSDictionary dictionaryWithObject:val forKey:@"sign"];
    [TalkingDataGA handleTDGAPushMessage:dic];
}
#endif

#pragma GCC diagnostic warning "-Wmissing-prototypes"
}
