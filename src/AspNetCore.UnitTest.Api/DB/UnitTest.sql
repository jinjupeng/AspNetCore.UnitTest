DROP TABLE IF EXISTS `Users`;
CREATE TABLE `Users` (
  `Id` int(11) NOT NULL AUTO_INCREMENT COMMENT '主键',
  `Email` varchar(255) DEFAULT NULL COMMENT '邮箱',
  `UserName` varchar(20) DEFAULT NULL COMMENT '用户名称',
  `Mobile` varchar(11) DEFAULT NULL COMMENT '手机号',
  `Age` int(11) DEFAULT NULL COMMENT '年龄',
  `Gender` int(1) DEFAULT '0' COMMENT '性别',
  `Avatar` varchar(255) DEFAULT NULL COMMENT '头像',
  `Salt` varchar(255) DEFAULT NULL COMMENT '加盐',
  `Password` varchar(255) DEFAULT NULL COMMENT '密码',
  `IsDelete` int(2) DEFAULT '0' COMMENT '0-正常 1-删除',
  `CreateTime` datetime DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
  PRIMARY KEY (`Id`),
  UNIQUE KEY `USER_MOBILE_INDEX` (`Mobile`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=10000 DEFAULT CHARSET=utf8mb4 COMMENT='用户信息表';